/*
    Most of the implementation is inspired of the NuTiled implementation of DotTiled
    source: https://github.com/differenceclouds/NuTiled
    he's really cool and awesome!
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using DotTiled.Serialization;
using Juegazo.CustomTiledTypes;
using Juegazo.CustomTiledTypesImplementation;
using Juegazo.Map.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ToolsUtilities;
using Color = Microsoft.Xna.Framework.Color;

namespace Juegazo.Map
{

    public class TiledMap
    {
        GraphicsDevice graphicsDevice;
        public DotTiled.Map Map { get; }
        public string MapFilePath { get; }
        public string TiledProjectDirectory { get; }
        public string MapFileDirectory => Path.Combine(TiledProjectDirectory, Path.GetDirectoryName(MapFilePath));
        public uint Width => Map.Width;
        public uint Height => Map.Height;
        public uint TileHeight => Map.TileHeight;
        public uint TileWidth => Map.TileWidth;
        public int TILESIZE { get; }

        public Dictionary<string, Vector2> EntityPositionerByName { get; } = new();
        public Dictionary<TileObject, TiledTypesUsed> MapObjectToType { get; } = new();
        public Dictionary<ObjectLayer, CollisionBlockObjectLayer> MapObjectLayerToClass { get; } = new();
        List<Block> blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(Block)) && !t.IsAbstract)
                .Select(t => (Block)Activator.CreateInstance(t))
                .ToList();
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public Dictionary<TileObject, Block> dynamicBlocks = new();
        public Dictionary<Vector2, Block> collisionLayer { get; } = new(); //maybe Vector2 should be changed to Point
        public readonly List<ICustomTypeDefinition> CustomTypeDefinitions = new();

        public Dictionary<uint, Tileset> TilesetsByGID { get; } = new();
        public Dictionary<string, Tileset> TilesetsByName { get; } = new();
        public Dictionary<Tileset, Texture2D> TilemapTextures { get; } = new();
        public Camera camera { get; protected set; }

        public uint GID_Count
        {
            get
            {
                var last_tileset = Map.Tilesets[Map.Tilesets.Count - 1];
                return last_tileset.FirstGID + last_tileset.TileCount;
            }
        }
        public (Tileset tileset, uint tileID) GetTileID(TileLayer tileLayer, Point coord)
        {
            var gid = GetTileGID(tileLayer, coord);
            var tileset = TilesetsByGID[gid];
            return (tileset, gid - tileset.FirstGID);
        }
        public uint GetTileGID(TileLayer tileLayer, Point coord)
        {
            var gids = GetLayerGIDs(tileLayer);
            return gids[coord.Y * tileLayer.Width + coord.X];
        }
        public static uint[] GetLayerGIDs(TileLayer layer)
        {
            Data data = layer.Data;
            return data.GlobalTileIDs;
        }

        //temporal
        public List<Block> blocksTemp { get; } = new();

        public TiledMap(GraphicsDevice graphicsDevice, string projectDirectory, string mapFilePath, int TILESIZE, List<ICustomTypeDefinition> typeDefinitions, Camera camera)
        {
            this.graphicsDevice = graphicsDevice;
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            TiledProjectDirectory = projectDirectory;
            MapFilePath = mapFilePath;

            var loader = Loader.DefaultWith(customTypeDefinitions: typeDefinitions);
            Map = loader.LoadMap(Path.Combine(TiledProjectDirectory, MapFilePath));
            this.TILESIZE = TILESIZE;
            InitTilesets(Map.Tilesets, TiledProjectDirectory);
            InitImportantLayers(Map.Layers);
            InitInteractiveObjectLayers(Map.Layers);
        }

        private void InitInteractiveObjectLayers(List<BaseLayer> layers)
        {
            foreach (var layer in layers.OfType<ObjectLayer>())
            {
                if (!MapObjectLayerToClass.TryGetValue(layer, out var objectLayerClass)) continue;

                foreach (var tileObject in layer.Objects.OfType<TileObject>())
                {
                    if (!MapObjectToType.TryGetValue(tileObject, out var tiledType)) continue;

                    Block block = tiledType.createBlock(tileObject, TILESIZE, Map);

                    Vector2 position = new((int)(tileObject.X / TileWidth), (int)(tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight)); //OMFG I FUCKING FORGOT ABOUT THIS I ALMOST FUCKED THIS SHIT UP
                    if (objectLayerClass.canOverrideCollisionLayer)
                    {
                        collisionLayer[position] = block;
                        if (block is Blocks.Key)
                        {
                            Console.WriteLine($"key position: {position}");
                        }
                    }
                    else if (block is Blocks.MovementBlock movementBlock)
                    {
                        dynamicBlocks[tileObject] = movementBlock;
                    }
                }
            }
        }

        private void InitImportantLayers(List<BaseLayer> layers)
        {

            foreach (var layer in layers)
            {
                switch (layer.Class)
                {
                    case "Collision Tile Layer":
                        TileLayer tileLayer = (TileLayer)layer;
                        CreateCollisionLayer(tileLayer);
                        break;
                    case "Entity Spawner":
                        ObjectLayer objectLayer = (ObjectLayer)layer;
                        AddImportantPositions(objectLayer);
                        break;
                    case "Collision Blocks Object Layer":
                        MapObjectLayerToClass[(ObjectLayer)layer] = layer.MapPropertiesTo<CollisionBlockObjectLayer>();
                        ObjectLayer objectLayer1 = (ObjectLayer)layer;
                        InitObjectLayer(objectLayer1);
                        break;
                    case "":
                        Console.WriteLine("not a class, ignored");
                        break;
                    default:
                        Console.WriteLine("not Implemented");
                        Console.WriteLine(layer.Name);
                        break;
                }
            }
        }
        //Make this a initObjectLayer, add all the propieties and the objects in a dictionary, then search again to get other propieties (like the rectangle of an object that needs it) and save it in the instance. With that you now can modify the collisionLayer with whatever source you need.
        private void InitObjectLayer(ObjectLayer objectLayer)
        {
            List<uint> unimplementedThings = new();
            IEnumerable<uint> objectProperties;
            foreach (var obj in objectLayer.Objects)
            {
                if (!(obj is TileObject)) continue;
                var tobj = (TileObject)obj;
                switch (obj.Type)
                {
                    case "MovementBlock":
                        var papu = obj.MapPropertiesTo<CustomTiledTypes.MovementBlock>();

                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.MovementBlock(papu);
                        break;
                    case "DamageBlock":

                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        var damage = obj.MapPropertiesTo<CustomTiledTypes.DamageBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.DamageBlock(damage);
                        break;
                    case "CheckPointBlock":

                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        var checkPoint = obj.MapPropertiesTo<CustomTiledTypes.CheckPointBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.CheckPointBlock(checkPoint);
                        break;
                    case "CompleteLevelBlock":
                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        var complete = obj.MapPropertiesTo<CustomTiledTypes.CompleteLevelBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.CompleteLevelBlock(complete);
                        break;
                    case "VerticalBoostBlock":
                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        var boost = obj.MapPropertiesTo<CustomTiledTypes.VerticalBoostBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.VerticalBoostBlock(boost);
                        break;
                    case "JumpWallBlock":
                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        var jblock = obj.MapPropertiesTo<CustomTiledTypes.JumpWallBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.JumpWallBlock(jblock);
                        break;
                    case "Key":
                        var key = obj.MapPropertiesTo<CustomTiledTypes.Key>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.Key(key);
                        break;
                    case "DoorBlock":
                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        var doorBlock = obj.MapPropertiesTo<CustomTiledTypes.DoorBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.DoorBlock(doorBlock);
                        break;
                    default:
                        Console.WriteLine("not a class or not implemented");
                        break;
                }
            }
            foreach (var obj in objectLayer.Objects)
            {
                foreach (uint idNeeded in unimplementedThings)
                {
                    if (obj.ID == idNeeded)
                    {
                        foreach (var o in MapObjectToType.Values)
                        {
                            o.getNeededObjectPropeties(obj, TILESIZE, Map);
                        }
                    }
                }
            }
        }

        private void AddImportantPositions(ObjectLayer objectLayer)
        {
            foreach (var objectObject in objectLayer.Objects)
            {
                float x = objectObject.X / Map.TileWidth * TILESIZE;
                float y = objectObject.Y / Map.TileHeight * TILESIZE;
                EntityPositionerByName[objectObject.Type] = new Vector2(x, y);
            }
        }

        public void drawTileLayer(SpriteBatch spriteBatch, TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs; //get the csv
            for (int i = 0; i < data.Length; i++)
            {
                uint value = data[i];
                if (value == 0) continue; //air data
                value--;
                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);
                Vector2 position = new(x, y);
                Tileset atlasImage = TilesetsByGID[value]; //get the atlas image from dictionary
                if (atlasImage.Image.HasValue)
                {
                    DrawTile(spriteBatch, value, position, atlasImage);
                }
            }
        }

        private void CreateCollisionLayer(TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs;
            for (int i = 0; i < data.Length; i++)
            {
                uint value = data[i];
                if (value == 0) continue;
                value--;
                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);
                Vector2 position = new(x, y);

                foreach (Block block in blocks)
                {
                    if (value == block.value)
                    {
                        Block blockk = (Block)Activator.CreateInstance(block.GetType());
                        blockk.collider = new((int)position.X * TILESIZE,
                                              (int)position.Y * TILESIZE,
                                              TILESIZE,
                                              TILESIZE);
                        collisionLayer[position] = blockk;
                        blocksTemp.Add(blockk);
                    }
                }
            }
        }

        private void DrawTile(SpriteBatch spriteBatch, uint value, Vector2 position, Tileset atlasImage)
        {
            Rectangle desRectangle = getDestinationRectangle(position);
            if (!IsVisible(desRectangle)) return;
            Texture2D texture = TilemapTextures[atlasImage];
            Rectangle srcRectangle = GetSourceRect(value, atlasImage);
            spriteBatch.Draw(texture, desRectangle, srcRectangle, Color.White);
        }

        private Rectangle getDestinationRectangle(Vector2 position)
        {
            return new((int)position.X * TILESIZE, (int)position.Y * TILESIZE, TILESIZE, TILESIZE);
        }

        public static Rectangle GetSourceRect(uint id, Tileset tileset)
        {
            uint col = id % tileset.Columns;
            uint row = id / tileset.Columns;

            uint x = tileset.Margin + (col * (tileset.TileWidth + tileset.Spacing));
            uint y = tileset.Margin + (row * (tileset.TileHeight + tileset.Spacing));

            return new Rectangle((int)x, (int)y, (int)tileset.TileWidth, (int)tileset.TileHeight);
        }

        private void InitTilesets(List<Tileset> tilesets, string path)
        {
            foreach (Tileset tileset in tilesets)
            {
                TilesetsByName.Add(tileset.Name, tileset);
                if (tileset.Image.HasValue)
                {
                    TilemapTextures.Add(tileset, LoadImage(graphicsDevice, path, tileset.Image));
                }
            }

            for (uint gid = 1; gid < GID_Count; gid++)
            {
                var (tileset, tile) = GetTilesetFromGID(gid, true);
                TilesetsByGID.Add(gid, tileset);
            }

        }
        public (Tileset tileset, Tile tile) GetTilesetFromGID(uint gid, bool safe = false)
        {
            foreach (Tileset tileset in Map.Tilesets)
            {
                if (tileset.Image.HasValue)
                {
                    if (gid >= tileset.FirstGID && gid < tileset.FirstGID + tileset.TileCount)
                    {
                        return (tileset, null);
                    }
                }
                else
                {
                    //Collection tilesets can have missing IDs and IDs greater than the tile count
                    foreach (Tile tile in tileset.Tiles)
                    {
                        if (tile.ID == gid - tileset.FirstGID)
                        {
                            return (tileset, tile);
                        }
                    }
                }
            }
            if (!safe)
            {
                if (gid < 1000000)
                    throw new Exception($"No tileset with gid {gid} exists.");
                else
                    throw new Exception($"gid {gid} doesn't have FlippingFlag bits stripped");
            }
            else
            {
                return (null, null);
            }
        }
        public void DrawLayerGroup(SpriteBatch spriteBatch, List<BaseLayer> layers)
        {
            foreach (BaseLayer layer in layers)
            {
                if (!layer.Visible) continue;
                // Vector2 parallax = new Vector2(layer.ParallaxX, layer.ParallaxY); //TODO: add parallax effect
                switch (layer)
                {
                    case Group group:
                        DrawLayerGroup(spriteBatch, group.Layers);
                        break;
                    case TileLayer tileLayer:
                        if (tileLayer.Class == "Collision Tile Layer")
                        {
                            DrawTileCollisionLayer(spriteBatch, tileLayer);
                            break;
                        }
                        drawTileLayer(spriteBatch, tileLayer);
                        break;
                    case ObjectLayer objectLayer:
                        DrawObjectLayer(spriteBatch, objectLayer);
                        break;
                }
            }
        }

        private void DrawTileCollisionLayer(SpriteBatch spriteBatch, TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs; //get the csv
            for (int i = 0; i < data.Length; i++)
            {
                uint value = data[i];
                if (value == 0) continue; //air data
                value--;
                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);

                collisionLayer.TryGetValue(new(x, y), out var block);
                if (!IsVisible(block.collider)) continue; //skips if block is not visible
                Tileset atlasImage = TilesetsByGID[value]; //get the atlas image from dictionary
                if (atlasImage.Image.HasValue)
                {
                    Texture2D texture = TilemapTextures[atlasImage];
                    Rectangle srcRectangle = GetSourceRect(value, atlasImage);
                    spriteBatch.Draw(texture, srcRectangle, srcRectangle, Color.White);
                    block.Draw(spriteBatch, texture, srcRectangle);
                }
            }
        }

        private void DrawObjectLayer(SpriteBatch spriteBatch, ObjectLayer objectLayer)
        {
            if (!objectLayer.Visible) return;
            foreach (var obj in objectLayer.Objects)
            {
                switch (obj)
                {
                    case TileObject tileObject:
                        DrawTileObject(spriteBatch, tileObject, objectLayer);
                        break;
                    default:
                        // Console.WriteLine("My fault gang"); //TODO: create object Draw for debugging
                        break;
                }
            }
        }

        private void DrawTileObject(SpriteBatch spriteBatch, TileObject tileObject, ObjectLayer objectLayer)
        {
            Rectangle destRect = GetObjectDestinationRectangle(tileObject);
            if (!IsVisible(destRect))
                return;
            Tileset tileset = TilesetsByGID[tileObject.GID];
            uint id = tileObject.GID - tileset.FirstGID;
            if (tileset.Image.HasValue)
            {
                Texture2D texture = TilemapTextures[tileset];
                Rectangle srcRect = GetSourceRect(id, tileset);

                if (dynamicBlocks.TryGetValue(tileObject, out var dynBlock))
                {
                    dynBlock.Draw(spriteBatch, texture, srcRect);
                }
                else if (collisionLayer.TryGetValue(
                    new Vector2((int)(tileObject.X / TileWidth), (int)(tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight)),
                    out var colBlock))
                {
                    colBlock.Draw(spriteBatch, texture, srcRect);
                }
                else
                {
                    spriteBatch.Draw(texture, destRect, srcRect, Color.White);
                }
            }
        }

        // Helper method to determine if the destination rectangle is visible in the viewport.
        private bool IsVisible(Rectangle destRect)
        {
            return camera.IsRectangleVisible(destRect);
        }

        private Rectangle GetObjectDestinationRectangle(DotTiled.Object tileObject)
        {
            int x = (int)(tileObject.X / TileWidth * TILESIZE);
            int y = (int)(((tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight)) * TILESIZE);
            Rectangle destRect = new(x, y, TILESIZE, TILESIZE);
            return destRect;
        }

        public static Texture2D LoadImage(GraphicsDevice graphicsDevice, string caller_directory, Image image)
        {
            string relative_path = image.Source;
            string file = Path.GetFileName(relative_path);
            string folder = Path.GetDirectoryName(relative_path);
            string path = Path.Combine(caller_directory, folder, file);

            return LoadImage(graphicsDevice, path, caller_directory);
        }
        public static Texture2D LoadImage(GraphicsDevice graphicsDevice, string path, string projectDirectory)
        {
            path = Path.Combine(projectDirectory, path);
            return Texture2D.FromFile(graphicsDevice, path, DefaultColorProcessors.PremultiplyAlpha);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawLayerGroup(spriteBatch, Map.Layers);
        }
    }
}