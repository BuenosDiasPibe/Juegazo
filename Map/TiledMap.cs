/*
    Most of the implementation is inspired of the NuTiled implementation of DotTiled
    source: https://github.com/differenceclouds/NuTiled
    he's really cool and awesome!

    TODO: REWRITE LOTS OF STUFF THERE'S A LOT OF REPETITION WHAT ARE YOU DOING?????
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using DotTiled.Serialization;
using Juegazo.Components;
using Juegazo.CustomTiledTypes;
using Juegazo.CustomTiledTypesImplementation;
using Juegazo.Map.Blocks;
using Juegazo.Map.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using ToolsUtilities;
using Color = Microsoft.Xna.Framework.Color;

namespace Juegazo.Map
{

    public class TiledMap
    {
        GraphicsDevice graphicsDevice;
        GumService gum;
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
        List<Block> blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(Block)) && !t.IsAbstract)
                .Select(t => (Block)Activator.CreateInstance(t))
                .ToList();
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public Dictionary<TileObject, Block> dynamicBlocks = new();
        public Dictionary<Vector2, Block> collisionLayer { get; } = new(); //maybe Vector2 should be changed to Point
        public List<Entity> entities { get; set; } = new();
        public readonly List<ICustomTypeDefinition> CustomTypeDefinitions = new();
        public Dictionary<TileObject, Tile> MapTileObjectToTile = new();

        public Dictionary<uint, Tileset> TilesetsByGID { get; } = new();
        public Dictionary<Tileset, Texture2D> TilemapTextures { get; } = new();
        public Dictionary<string, ImageLayer> imageLayerByName { get; } = new();
        public Dictionary<ImageLayer, Texture2D> ImageLayerTexture { get; } = new();
        public Camera camera { get; protected set; }

        public uint GID_Count
        {
            get
            {
                var last_tileset = Map.Tilesets[Map.Tilesets.Count - 1];
                return last_tileset.FirstGID + last_tileset.TileCount;
            }
        }
        public TiledMap(GraphicsDevice graphicsDevice, string projectDirectory, string mapFilePath, int TILESIZE, List<ICustomTypeDefinition> typeDefinitions, Camera camera, GumService gum)
        {
            this.graphicsDevice = graphicsDevice;
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            this.gum = gum;
            TiledProjectDirectory = projectDirectory;
            MapFilePath = mapFilePath;

            var loader = Loader.DefaultWith(customTypeDefinitions: typeDefinitions);
            Map = loader.LoadMap(Path.Combine(TiledProjectDirectory, MapFilePath)); //this can throw an exception if a layer is not visible, because tiled puts that as 0, idfk why

            this.TILESIZE = TILESIZE;
            InitTilesets(Map.Tilesets, TiledProjectDirectory);
            InitLayerGroup(Map.Layers);
        }

        public void InitLayerGroup(List<BaseLayer> layers)
        {
            foreach (var layer in layers)
            {
                AllLayersByName[layer.Name] = layer;
                switch (layer)
                {
                    case ImageLayer imageLayer:
                        imageLayerByName[imageLayer.Name] = imageLayer;
                        if (!imageLayer.Image.HasValue) break;
                        Texture2D texture = LoadImage(graphicsDevice, MapFileDirectory, imageLayer.Image);
                        ImageLayerTexture[imageLayer] = texture;
                        break;
                }
                switch (layer.Class)
                {
                    case "Collision Tile Layer":
                        TileLayer tileLayer = (TileLayer)layer;
                        CreateCollisionLayer(tileLayer);
                        break;
                    case "Entity Spawner":
                        ObjectLayer objectLayer = (ObjectLayer)layer;
                        CreateEntities(objectLayer);
                        break;
                    case "Collision Blocks Object Layer":
                        ObjectLayer objectLayer1 = (ObjectLayer)layer;
                        InitObjectLayer(objectLayer1);
                        InitCollisionObjectLayer(objectLayer1);
                        break;
                    default:
                        if(layer.Class != "")
                        {
                            Console.WriteLine($"layer \"{layer.Class}\" not implemented / Type: \"{layer.GetType()}\"");
                        }
                        break;
                }
            }
        }
        private void CreateCollisionLayer(TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs;
            for (int i = 0; i < data.Length; i++)
            {
                uint TileID = data[i];
                if (TileID == 0) continue;
                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);
                Vector2 position = new(x, y);
                var (tileset, tile) = GetTilesetFromGID(TileID, true);
                if (tile == null)
                {
                    Console.WriteLine($"Not possible to create an object. Tile.ID = {TileID}");
                    continue;
                }
                bool blockPlaced = false;
                foreach (Block block in blocks)
                {
                    if (block.type == tile.Type)
                    {
                        Block blockk = (Block)Activator.CreateInstance(block.GetType());
                        blockk.collider = new((int)position.X * TILESIZE,
                                              (int)position.Y * TILESIZE,
                                              TILESIZE,
                                              TILESIZE);
                        blockk.tile = tile;
                        blockk.Start();
                        collisionLayer[position] = blockk;
                        blockPlaced = true;
                        break;
                    }
                }
                if (!blockPlaced)
                    CreatePowerUpEntity(getDestinationRectangle(position), tileset, tile);
            }
        }
        private void InitCollisionObjectLayer(ObjectLayer layer)
        {
                var objectLayerClass = layer.MapPropertiesTo<CollisionBlockObjectLayer>();
                foreach (var tileObject in layer.Objects.OfType<TileObject>())
                {
                    if (!MapObjectToType.TryGetValue(tileObject, out var tiledType)) continue;
                    Tile tile = MapTileObjectToTile[tileObject];

                    Block block = tiledType.createBlock(tileObject, TILESIZE, Map);
                    block.tile = tile;
                    if (tile == null) Console.WriteLine($"block: {block.ToString()}");
                    if (block == null)
                    {
                        Console.WriteLine("not block created");
                        continue;
                    }
                    block.Start(); //i always forget to add this here...

                    Vector2 position = new((int)(tileObject.X / TileWidth), (int)(tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight));

                    if (objectLayerClass.canOverrideCollisionLayer)
                    {
                        collisionLayer[position] = block;
                    }
                    else
                    {
                        dynamicBlocks[tileObject] = block;
                    }
                }
        }
        private void InitObjectLayer(ObjectLayer objectLayer)
        {
            List<uint> unimplementedThings = new();
            IEnumerable<uint> objectProperties;
            foreach (var obj in objectLayer.Objects)
            {
                if (!(obj is TileObject)) continue;

                var tobj = (TileObject)obj;
                var (tileset, tileData) = GetTilesetFromGID(tobj.GID, true);
                if (tileData != null)
                {
                    if (string.IsNullOrEmpty(tobj.Type))
                    {
                        tobj.Type = tileData.Type;
                    }
                    if (tobj.Properties.Count == 0 && tileData.Properties.Count > 0)
                    {
                        foreach (var prop in tileData.Properties)
                        {
                            tobj.Properties.Add(prop);
                        }
                    }
                }
                MapTileObjectToTile[tobj] = tileData;
                switch (obj.Type)
                {
                    case "SpeedUp":
                        var pap = obj.MapPropertiesTo<CustomTiledTypes.SpeedUp>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.SpeedUp(pap);
                        break;
                    case "Collision Block":
                        var paap = obj.MapPropertiesTo<CustomTiledTypes.CollisionBlock>();
                        MapObjectToType[tobj] = new CustomTiledTypesImplementation.CollisionBlock(paap);
                        break;
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
                        Console.WriteLine($"damageBlock tile data: {tileData}");
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
                        if(!CreatePowerUpEntity(obj, tileset, tileData))
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

        private bool CreatePowerUpEntity(DotTiled.Object obj, Tileset tileset, Tile tileData)
        {
            Entity entity = new Entity(TilemapTextures[tileset], GetSourceRect(tileData.ID, tileset), GetObjectDestinationRectangle(obj), 1, Color.Red);
            PowerUpGiverComponent c = new();
            switch (tileData.Type)
            {
                case "DoubleJump":
                    var jumpData = obj.MapPropertiesTo<DoubleJump>() ?? new DoubleJump();
                    c.componentGived = new DoubleJumpComponent(jumpData);
                    break;
                default:
                    Console.WriteLine("type not added, skipped1 ");
                    break;
            }
            if (c.componentGived == null) return false;
            c.Start();
            entity.AddComponent(c.GetType(), c);
            Console.WriteLine("Created power up");

            entities.Add(entity);
            return true;
        }
        private void CreatePowerUpEntity(Rectangle destRect, Tileset tileset, Tile tileData)
        {
            Entity entity = new Entity(TilemapTextures[tileset], GetSourceRect(tileData.ID, tileset),destRect, 1, Color.White);
            PowerUpGiverComponent c = new();
            switch (tileData.Type)
            {
                case "DoubleJump":
                    c.componentGived = new DoubleJumpComponent(tileData.MapPropertiesTo<DoubleJump>());
                    break;
                default:
                    Console.WriteLine($"not case on collisionLayer, tile: {tileData.ID}");
                    break;
            }
            if (c.componentGived == null) return;
            c.Start();
            entity.AddComponent(c.GetType(), c);
            Console.WriteLine("Created power up");

            entities.Add(entity);
        }

        public void CreateEntities(ObjectLayer objectLayer)
        {
            foreach (var obj in objectLayer.Objects)
            {
                float x = obj.X / Map.TileWidth * TILESIZE;
                float y = obj.Y / Map.TileHeight * TILESIZE;
                EntityPositionerByName[obj.Type] = new Vector2(x, y); //get all entities positions even if they are not a tileObject (if player is a rectangle for example)

                if (!(obj is TileObject tile)) continue;

                var (tileset, tileData) = GetTilesetFromGID(tile.GID);
                if (tileData == null) continue;

                if (obj.Properties.Count == 0 && tileData.Properties.Count > 0)
                {
                    foreach (var prop in tileData.Properties)
                    {
                        obj.Properties.Add(prop);
                    }
                }

                if (tile.Type == "NPC" || tileData.Type == "NPC")
                {
                    var papu = tile.MapPropertiesTo<NPC>();

                    Texture2D atlasImage = TilemapTextures[tileset]; //this looks like shit but idk

                    Entity entity = new Entity(atlasImage, GetSourceRect(tile.GID, tileset), GetObjectDestinationRectangle(tile), 1, Color.White);
                    entity.AddComponents(new List<Component>
                    {
                        new NPCAnimationComponent(tileData),
                        new NPCComponent(camera, papu.name, papu.dialogStart, papu.dialogEnd, gum)
                    });
                    entities.Add(entity);
                }else if(tile.Type == "DoubleJump" || tileData.Type == "DoubleJump")
                {
                    CreatePowerUpEntity(obj, tileset, tileData);
                }
            }
        }

        public void drawTileLayer(GameTime gameTime,SpriteBatch spriteBatch, TileLayer tileLayer)
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
                if (atlasImage == null)
                {
                    Console.WriteLine($"tile {value} missing tileset");
                    continue;
                }
                if (atlasImage.Image.HasValue)
                {
                    DrawTile(gameTime, spriteBatch, value, position, atlasImage);
                }
            }
        }


        private void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, uint value, Vector2 position, Tileset atlasImage)
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
                foreach (Tile tile in tileset.Tiles)
                {
                    if (tile.ID == gid - tileset.FirstGID)
                    {
                        return (tileset, tile);
                    }
                }
                return (tileset, null);
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
        public void DrawLayerGroup(GameTime gameTime, SpriteBatch spriteBatch, List<BaseLayer> layers)
        {
            foreach (BaseLayer layer in layers)
            {
                if (!layer.Visible) continue;
                switch (layer)
                {
                    case Group group:
                        DrawLayerGroup(gameTime, spriteBatch, group.Layers);
                        break;
                    case TileLayer tileLayer:
                        if (tileLayer.Class == "Collision Tile Layer")
                        {
                            DrawTileCollisionLayer(gameTime, spriteBatch, tileLayer);
                            break;
                        }
                        drawTileLayer(gameTime, spriteBatch, tileLayer);
                        break;
                    case ObjectLayer objectLayer:
                        if (objectLayer.Class == "Entity Spawner") continue;
                        DrawObjectLayer(gameTime, spriteBatch, objectLayer);
                        break;
                    case ImageLayer imageLayer:
                        DrawImageLayer(gameTime, spriteBatch, imageLayer);
                        break;
                    default:
                        Console.WriteLine($"layer type {layer.GetType()} not implemented");
                        break;
                }
            }
        }
        private void DrawImageLayer(GameTime gameTime,SpriteBatch spriteBatch, ImageLayer imageLayer)
        {
            if (!imageLayer.Image.HasValue) return; //TODO: add an error image
            Texture2D texture = ImageLayerTexture[imageLayer];
            bool repeatX = imageLayer.RepeatX;
            bool repeatY = imageLayer.RepeatY;
            Vector2 offset = new(imageLayer.OffsetX, imageLayer.OffsetY);
            Rectangle srcRect;
            Rectangle destRect;
            Rectangle viewport_bounds = camera.ViewPortRectangle;
            switch (repeatX, repeatY)
            {
                default:
                case (false, false):
                    {
                        srcRect = texture.Bounds;
                        destRect = new Rectangle((int)(imageLayer.X + imageLayer.ParallaxX * camera.ViewPortRectangle.X), (int)(imageLayer.Y.Value + imageLayer.ParallaxY * camera.ViewPortRectangle.Y), texture.Width, texture.Height);
                        break;
                    }
                case (true, true):
                    {
                        destRect = camera.ViewPortRectangle;
                        srcRect = camera.ViewPortRectangle;
                        break;
                    }
                case (true, false):
                    {
                        destRect = new Rectangle(viewport_bounds.X, (int)imageLayer.OffsetY, viewport_bounds.Width, texture.Height);
                        srcRect = new Rectangle(destRect.X, 0, destRect.Width, texture.Height);
                        break;
                    }
                case (false, true):
                    {
                        destRect = new Rectangle((int)offset.X, viewport_bounds.Y, texture.Width, viewport_bounds.Height);
                        srcRect = new Rectangle(0, destRect.Y, texture.Width, destRect.Height);
                        break;
                    }
            }
            spriteBatch.Draw(texture, destRect, srcRect, Color.White);
        }

        private void DrawTileCollisionLayer(GameTime gameTime,SpriteBatch spriteBatch, TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs; //get the csv
            for (int i = 0; i < data.Length; i++)
            {
                uint value = data[i];
                if (value == 0) continue; //air data
                value--;
                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);
                var (atlasImage, tile) = GetTilesetFromGID(value + 1, true);
                if (tile == null)
                {
                    DrawTile(gameTime, spriteBatch, value, new(x, y), atlasImage);
                    continue;
                }
                

                collisionLayer.TryGetValue(new(x, y), out var block);
                if (block == null)
                {
                    if (tile.Type == "DoubleJump") continue;
                    DrawTile(gameTime, spriteBatch, value, new(x, y), atlasImage); //if its not a block, show the tile anyway
                    continue;
                }
                if (!IsVisible(block.collider)) continue; //skips if block is not visible

                if (atlasImage.Image.HasValue)
                {
                    Texture2D texture = TilemapTextures[atlasImage];
                    Rectangle srcRectangle = GetSourceRect(value, atlasImage);

                    block.Draw(gameTime, spriteBatch, texture, srcRectangle);
                } //TODO: add case when blocks have a different tileset
            }
        }

        private void DrawObjectLayer(GameTime gameTime,SpriteBatch spriteBatch, ObjectLayer objectLayer)
        {
            if (!objectLayer.Visible) return;
            foreach (var obj in objectLayer.Objects)
            {
                switch (obj)
                {
                    case TileObject tileObject:
                        DrawTileObject(gameTime, spriteBatch, tileObject, objectLayer);
                        break;
                    default:
                        // Console.WriteLine("My fault gang"); //TODO: create object Draw for debugging
                        break;
                }
            }
        }

        private void DrawTileObject(GameTime gameTime, SpriteBatch spriteBatch, TileObject tileObject, ObjectLayer objectLayer)
        {
            Rectangle destRect = GetObjectDestinationRectangle(tileObject);
            if (!IsVisible(destRect)) return;
            Tileset tileset = TilesetsByGID[tileObject.GID];
            uint id = tileObject.GID - tileset.FirstGID;
            if (tileset.Image.HasValue)
            {
                Texture2D texture = TilemapTextures[tileset];
                Rectangle srcRect = GetSourceRect(id, tileset);

                if (dynamicBlocks.TryGetValue(tileObject, out var dynBlock))
                {
                    dynBlock.Draw(gameTime, spriteBatch, texture, srcRect);
                }
                else if (collisionLayer.TryGetValue(
                    new Vector2((int)(tileObject.X / TileWidth), (int)(tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight)),
                    out var colBlock))
                {
                    colBlock.Draw(gameTime, spriteBatch, texture, srcRect);
                }
                else
                {
                    destRect = new((int)(tileObject.X / TileWidth * TILESIZE),
                                   (int)(((tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight)) * TILESIZE),
                                   (int)(tileObject.Width / TileWidth * TILESIZE),
                                   (int)(tileObject.Height / TileHeight * TILESIZE));
                    //spriteBatch.Draw(texture, destRect, srcRect, Color.White);
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
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawLayerGroup(gameTime, spriteBatch, Map.Layers);
        }
    }
}