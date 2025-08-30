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

        public Dictionary<Vector2, Block> collisionLayer { get; } = new();
        public Dictionary<string, Vector2> EntityPositionerByName { get; } = new();
        public Dictionary<DotTiled.Object, TiledTypesUsed> MapObjectToType { get; } = new();
        public Dictionary<DotTiled.ObjectLayer, CollisionBlockObjectLayer> MapObjectLayerToClass { get; } = new();
        List<Block> blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(Block)) && !t.IsAbstract)
                .Select(t => (Block)Activator.CreateInstance(t))
                .ToList();
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public Dictionary<DotTiled.Object, Block> dynamicBlocks = new();

        public readonly List<ICustomTypeDefinition> CustomTypeDefinitions = new();

        public Dictionary<uint, Tileset> TilesetsByGID { get; } = new();
        public Dictionary<string, Tileset> TilesetsByName { get; } = new();
        public Dictionary<Tileset, Texture2D> TilemapTextures { get; } = new();

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

        public TiledMap(GraphicsDevice graphicsDevice, string projectDirectory, string mapFilePath, int TILESIZE, List<ICustomTypeDefinition> typeDefinitions)
        {
            this.graphicsDevice = graphicsDevice;
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            this.graphicsDevice = graphicsDevice;
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
            foreach (var layer in layers)
            {
                if (!(layer is ObjectLayer)) continue;
                var objLayer = (ObjectLayer)layer;
                if (MapObjectLayerToClass.TryGetValue(objLayer, out var val) && !val.canOverrideCollisionLayer)
                {
                    foreach (var obj in objLayer.Objects)
                    {
                        if (MapObjectToType.TryGetValue(obj, out var value))
                        {
                            if (!(value is CustomTiledTypesImplementation.MovementBlock))
                            {
                                Console.WriteLine("fufufufck");
                            }
                            Blocks.MovementBlock block = new Blocks.MovementBlock(GetObjectDestinationRectangle(obj));
                            Blocks.MovementBlock b = (Blocks.MovementBlock)value.changeBlock(block);
                            Blocks.MovementBlock a = new(b.collider, b.initialBlockPosition, b.endBlockPosition);
                            dynamicBlocks[obj] = a;
                        }
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
                    default:
                        Console.WriteLine("still not implemented");
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
                switch (obj.Type)
                {
                    case "MovementBlock":
                        Console.WriteLine("MovementBock");
                        var papu = obj.MapPropertiesTo<CustomTiledTypes.MovementBlock>();

                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        MapObjectToType[obj] = new CustomTiledTypesImplementation.MovementBlock(papu);
                        break;
                    case "DamageBlock":
                        var damage = obj.MapPropertiesTo<CustomTiledTypes.DamageBlock>();

                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        MapObjectToType[obj] = new CustomTiledTypesImplementation.DamageBlock(damage);
                        break;
                    case "CheckPointBlock":
                        Console.WriteLine("fuck i forgot this one");
                        break;
                    case "CompleteLevelBlock":
                        // var complete = obj.MapPropertiesTo<CustomTiledTypes.CompleteLevelBlock>();
                        // unimplementedThings.Add(obj);
                        // MapObjectToType[obj] = new CustomTiledTypesImplementation.CompleteLevelBlock(complete);
                        Console.WriteLine("fuck i forgot completeblock");
                        break;
                    case "VerticalBoostBlock":
                        var boost = obj.MapPropertiesTo<CustomTiledTypes.VerticalBoostBlock>();

                        objectProperties = obj.Properties
                            .Where(p => p.Type == PropertyType.Object)
                            .Cast<ObjectProperty>()
                            .Select(op => op.Value);
                        unimplementedThings.AddRange(objectProperties);

                        MapObjectToType[obj] = new CustomTiledTypesImplementation.VerticalBoostBlock(boost);
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
                            o.getNeededObjectPropeties(obj);
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
            Texture2D texture = TilemapTextures[atlasImage];
            Rectangle srcRectangle = GetSourceRect(value, atlasImage);
            Rectangle desRectangle = getDestinationRectangle(position);
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
                // Vector2 parallax = new Vector2(layer.ParallaxX, layer.ParallaxY); //TODO: add parallax effect
                switch (layer)
                {
                    case Group group:
                        DrawLayerGroup(spriteBatch, group.Layers);
                        break;
                    case TileLayer tileLayer:
                        drawTileLayer(spriteBatch, tileLayer);
                        break;
                    case ObjectLayer objectLayer:
                        DrawObjectLayer(spriteBatch, objectLayer);
                        break;
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
            if (!tileObject.Visible) return;
            Tileset tileset = TilesetsByGID[tileObject.GID];
            uint id = tileObject.GID - tileset.FirstGID;
            if (tileset.Image.HasValue)
            {
                Texture2D texture = TilemapTextures[tileset];
                Rectangle srcRect = GetSourceRect(id, tileset);
                if (dynamicBlocks.TryGetValue(tileObject, out var _val))
                {
                    spriteBatch.Draw(texture, _val.collider, srcRect, Color.White);
                }
                Rectangle destRect = GetObjectDestinationRectangle(tileObject);
                spriteBatch.Draw(texture, destRect, srcRect, Color.White);
            }
        }

        private Rectangle GetObjectDestinationRectangle(DotTiled.Object tileObject)
        {
            int x = (int)(tileObject.X / TileWidth * TILESIZE);
            int y = (int)(((tileObject.Y / TileHeight) - 1) * TILESIZE);
            Rectangle destRect = new(x, y, TILESIZE, TILESIZE);
            return destRect;
        }

        public static Texture2D LoadImage(GraphicsDevice graphicsDevice, string caller_directory, Image image)
        {
            string relative_path = image.Source;
            string file = Path.GetFileName(relative_path);
            string folder = Path.GetDirectoryName(relative_path);
            string path = Path.Combine(caller_directory, folder, file);

            //TODO: fuck off
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