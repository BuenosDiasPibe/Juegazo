/*
    Most of the implementation is inspired of the NuTiled implementation of DotTiled
    source: https://github.com/differenceclouds/NuTiled
    he's really cool and awesome!

    TODO: REWRITE LOTS OF STUFF THERE'S A LOT OF REPETITION WHAT ARE YOU DOING?????
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
        Dictionary<string, Block> blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    return a.GetTypes();
                })
                .Where(t => t.IsSubclassOf(typeof(Block)) && !t.IsAbstract)
                .Select(t => (Block)Activator.CreateInstance(t))
                .ToDictionary(inst => inst.GetType().Name, inst => inst);
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public Dictionary<TileObject, Block> dynamicBlocks = new();
        public Dictionary<Vector2, Block> collisionLayer { get; } = new(); //maybe Vector2 should be changed to Point
        public List<Entity> entities { get; set; } = new();
        public readonly List<ICustomTypeDefinition> CustomTypeDefinitions = new();
        public Dictionary<TileObject, Tile> MapTileObjectToTile = new();

        public Dictionary<uint, Tileset> TilesetsByGID { get; } = new();
        public Dictionary<Tile, Texture2D> TileCollectionTextures { get; } = new();
        public Dictionary<uint, Tile> TilesByGID { get; } = new();
        public Dictionary<Tileset, Texture2D> TilemapTextures { get; } = new();
        public Dictionary<string, ImageLayer> imageLayerByName { get; } = new();
        public Dictionary<ImageLayer, Texture2D> ImageLayerTexture { get; } = new();

        // two GID_Counts because Collection tilesets can have missing IDs and IDs greater than the tile count. So GID_Count_Last tries to get all the possible GIDS by adding the last tile id to count for the missing ids
        public uint GID_Count_Last
        {
            get
            {
                var last_tileset = Map.Tilesets[Map.Tilesets.Count - 1];
                uint lastTileID = last_tileset.Tiles[last_tileset.Tiles.Count - 1].ID;
                return last_tileset.FirstGID + last_tileset.TileCount + lastTileID;
            }
        }
        public uint GID_Count
        {
            get
            {
                var last_tileset = Map.Tilesets[Map.Tilesets.Count - 1];
                return last_tileset.FirstGID + last_tileset.TileCount;
            }
        }

        public float cameraZoom = 1f;
        public bool levelBoundries = true;
        public TiledMap(GraphicsDevice graphicsDevice, string projectDirectory, string mapFilePath, int TILESIZE, List<ICustomTypeDefinition> typeDefinitions, GumService gum)
        {
            this.graphicsDevice = graphicsDevice;
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            this.graphicsDevice = graphicsDevice;
            this.gum = gum;
            TiledProjectDirectory = projectDirectory;
            MapFilePath = mapFilePath;

            var loader = Loader.DefaultWith(customTypeDefinitions: typeDefinitions);
            Map = loader.LoadMap(Path.Combine(TiledProjectDirectory, MapFilePath)); //this can throw an exception if a layer is not visible, because tiled puts that as 0, and its only on .tmx files
            if(Map.Class == "LevelPropieties")
            {
                var c = Map.MapPropertiesTo<LevelPropieties>();
                cameraZoom = c.zoom;
                levelBoundries = c.ClampCameraToBoundries;
            }
            this.TILESIZE = TILESIZE;
            var sw = new Stopwatch();
            sw.Start();
            InitTilesets(Map.Tilesets, TiledProjectDirectory);
            InitLayerGroup(Map.Layers);
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds}ms to render all the level.");
        }

        public void InitLayerGroup(List<BaseLayer> layers)
        {
            foreach (var layer in layers)
            {
                AllLayersByName[layer.Name] = layer;

                switch (layer)
                {
                    case Group g: //ayy casi me olvido :skull:
                        InitLayerGroup(g.Layers);
                        break;
                    case ImageLayer imageLayer:
                        imageLayerByName[imageLayer.Name] = imageLayer;
                        if (!imageLayer.Image.HasValue) break;
                        Texture2D texture = LoadImage(graphicsDevice, MapFileDirectory, imageLayer.Image);
                        ImageLayerTexture[imageLayer] = texture;
                        break;
                }
                var sw = new Stopwatch();
                switch (layer.Class)
                { 
                    case "Collision Tile Layer":
                        if (layer is not TileLayer tLayer)
                            throw new Exception($"layer \"{layer.Name}\" is using a TileLayer class when its a \"{layer.GetType()}\"");
                        sw.Start();
                        CreateCollisionLayer(tLayer);
                        sw.Stop();
                        Console.WriteLine($"{sw.ElapsedMilliseconds}ms to finish CreateCollisionLayer");
                        break;
                    case "Entity Spawner":
                        if (layer is not ObjectLayer objectLayer)
                            throw new Exception($"layer \"{layer.Name}\" is using a ObjectLayer class when its a \"{layer.GetType()}\"");
                        CreateEntities(objectLayer);
                        break;
                    case "Collision Blocks Object Layer":
                        if (layer is not ObjectLayer objectLayer1)
                            throw new Exception($"layer \"{layer.Name}\" is using a ObjectLayer class when its a \"{layer.GetType()}\"");
                        sw.Start();
                        InitObjectLayer(objectLayer1);
                        sw.Stop();
                        Console.WriteLine($"{sw.ElapsedMilliseconds}ms to finish InitObjectLayer");
                        break;
                    default:
                        if (layer.Class != "")
                        {
                            Console.WriteLine($"layer \"{layer.Class}\" not implemented / Type: \"{layer.GetType()}\"");
                        }
                        break;
                }
            }
        }
        //really expensive operation, should be pararellized
        private void CreateCollisionLayer(TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs;
            Console.WriteLine($"data: {data.Length}");
            for (int i = 0; i < data.Length; i++)
            {
                uint TileID = data[i];
                if (TileID == 0) continue;
                var tile = TilesByGID[TileID];
                if (tile == null)
                {
                    Console.WriteLine($"Not possible to create an object. Tile.ID = {TileID}");
                    continue;
                }
                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);
                Vector2 position = new(x, y);
                var tileset = TilesetsByGID[TileID];
                bool blockPlaced = false;
                if(blocks.TryGetValue(tile.Type, out var block))
                {
                    var propieties = MapObjectToPropieties(tile);
                    var type = block.GetType();
                    Block blockk;
                    if (propieties != null)
                    {
                        blockk = propieties.createBlock(TILESIZE, Map);
                    }
                    else
                    {
                        blockk = (Block)Activator.CreateInstance(type);
                    }
                    blockk.collider = getDestinationRectangle(position, tileset); //TODO: on TiledTypesUsed, add collider and/or position too.
                    blockk.tile = tile;
                    blockk.Start();
                    collisionLayer[position] = blockk;
                    blockPlaced = true;
                }
                if (!blockPlaced)
                    CreatePowerUpEntity(getDestinationRectangle(position, tileset), tileset, tile);
            }
        }
        private void InitCollisionObjectLayer(ObjectLayer layer)
        {
            var objectLayerClass = layer.MapPropertiesTo<CollisionBlockObjectLayer>();
            foreach (var tileObject in layer.Objects.OfType<TileObject>())
            {
                if (!MapObjectToType.TryGetValue(tileObject, out var tiledType)) continue;
                if (!MapTileObjectToTile.TryGetValue(tileObject, out var tile)) continue;

                Block block = tiledType.createBlock(TILESIZE, Map, tileObject);
                block.tile = tile;
                if (block == null)
                {
                    Console.WriteLine("not block created");
                    continue;
                }
                block.Start();
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
        /// <summary>
        /// Checks if any tile.Type is a block, entity or is null, change the switch statement to make this work for you, im sorry
        /// </summary>
        /// <param name="objectLayer"></param>
        private void InitObjectLayer(ObjectLayer objectLayer)
        {
            List<uint> unimplementedThings = new();
            IEnumerable<uint> objectProperties;
            foreach (var obj in objectLayer.Objects)
            {
                if (!(obj is TileObject)) continue;

                var tobj = (TileObject)obj;
                var tileset = TilesetsByGID[tobj.GID];
                var tileData = TilesByGID[tobj.GID];
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

                (bool flowControl, objectProperties) = MapObjectToPropieties(unimplementedThings, obj, tobj, tileset, tileData);
                if (!flowControl)
                {
                    continue;
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

            InitCollisionObjectLayer(objectLayer);
        }

        // THOU SHALL NOT SEE WHAT'S INSIDE THIS DISGUSTING GARBAGE, it still works tho
        private (bool flowControl, IEnumerable<uint> value) MapObjectToPropieties(List<uint> unimplementedThings, DotTiled.Object obj, TileObject tobj, Tileset tileset, Tile tileData)
        {
            IEnumerable<uint> objectProperties;
            // dynamic mapping: look for types in CustomTiledTypes and implementations in CustomTiledTypesImplementation
            string typeName = obj.Type;
            if (string.IsNullOrEmpty(typeName))
            {
                Console.WriteLine($"{tileset?.Name} nothing here...");
                return (flowControl: false, value: null);
            }

            // collect any object property references (some types need them)
            objectProperties = obj.Properties
                .Where(p => p.Type == PropertyType.Object)
                .Cast<ObjectProperty>()
                .Select(op => op.Value);
            if (objectProperties.Any())
            {
                unimplementedThings.AddRange(objectProperties);
            }

            // find the property type (Juegazo.CustomTiledTypes.<typeName>)
            Type propType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .FirstOrDefault(t => t.Name == typeName && t.Namespace != null && t.Namespace.EndsWith("CustomTiledTypes"));

            object mappedProps = null;
            if (propType != null)
            {
                // invoke MapPropertiesTo<T>() dynamically
                var mapMethod = obj.GetType()
                    .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                    .FirstOrDefault(m => m.Name == "MapPropertiesTo" && m.IsGenericMethodDefinition && m.GetParameters().Length == 0)
                    ?.MakeGenericMethod(propType);
                if (mapMethod != null)
                {
                    mappedProps = mapMethod.Invoke(obj, null);
                }
                // ensure we have an instance (fallback)
                if (mappedProps == null)
                {
                    try { mappedProps = Activator.CreateInstance(propType); }
                    catch { mappedProps = null; }
                }
            }

            // find the implementation type (Juegazo.CustomTiledTypesImplementation.<typeName>)
            Type implType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .FirstOrDefault(t => t.Name == typeName && t.Namespace != null && t.Namespace.EndsWith("CustomTiledTypesImplementation"));

            if (implType != null)
            {
                object implInstance = null;

                // prefer a ctor that accepts the mapped props type
                if (mappedProps != null)
                {
                    var preferredCtor = implType.GetConstructors()
                        .FirstOrDefault(c =>
                        {
                            var ps = c.GetParameters();
                            return ps.Length == 1 && ps[0].ParameterType.IsAssignableFrom(mappedProps.GetType());
                        });
                    if (preferredCtor != null)
                    {
                        implInstance = preferredCtor.Invoke(new[] { mappedProps });
                    }
                }

                // try parameterless ctor
                if (implInstance == null)
                {
                    var defaultCtor = implType.GetConstructor(Type.EmptyTypes);
                    if (defaultCtor != null)
                        implInstance = defaultCtor.Invoke(null);
                }

                // last resort: try first ctor with whatever we have
                if (implInstance == null)
                {
                    var anyCtor = implType.GetConstructors().FirstOrDefault();
                    if (anyCtor != null)
                    {
                        var parms = anyCtor.GetParameters();
                        var args = parms.Select(p => p.ParameterType.IsInstanceOfType(mappedProps) ? mappedProps : (p.HasDefaultValue ? p.DefaultValue : null)).ToArray();
                        try { implInstance = anyCtor.Invoke(args); } catch { implInstance = null; }
                    }
                }

                if (implInstance is TiledTypesUsed tiledImpl)
                {
                    MapObjectToType[tobj] = tiledImpl;
                }
                else
                {
                    Console.WriteLine($"implementation for \"{typeName}\" does not implement TiledTypesUsed");
                }
            }
            else
            {
                // fallback to previous behaviour for unknown types: try creating powerup or log
                if (!CreatePowerUpEntity(obj, tileset, tileData))
                    Console.WriteLine($"class \"{obj.Type}\" not implemented");
            }

            return (flowControl: true, value: null);
        }
        private TiledTypesUsed MapObjectToPropieties(Tile tileData)
        {
            string typeName = tileData.Type;
            if (string.IsNullOrEmpty(typeName))
            {
                Console.WriteLine($"no data type added");
                return null;
            }
            Type propType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .FirstOrDefault(t => t.Name == typeName && t.Namespace != null && t.Namespace.EndsWith("CustomTiledTypes"));

            object mappedProps = null;
            if (propType != null)
            {
                // invoke MapPropertiesTo<T>() dynamically
                var mapMethod = tileData.GetType()
                    .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                    .FirstOrDefault(m => m.Name == "MapPropertiesTo" && m.IsGenericMethodDefinition && m.GetParameters().Length == 0)
                    ?.MakeGenericMethod(propType);
                if (mapMethod != null)
                {
                    mappedProps = mapMethod.Invoke(tileData, null);
                }
                // ensure we have an instance (fallback)
                if (mappedProps == null)
                {
                    try { mappedProps = Activator.CreateInstance(propType); }
                    catch { mappedProps = null; }
                }
            }

            // find the implementation type (Juegazo.CustomTiledTypesImplementation.<typeName>)
            Type implType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .FirstOrDefault(t => t.Name == typeName && t.Namespace != null && t.Namespace.EndsWith("CustomTiledTypesImplementation"));

            if (implType != null)
            {
                object implInstance = null;

                // prefer a ctor that accepts the mapped props type
                if (mappedProps != null)
                {
                    var preferredCtor = implType.GetConstructors()
                        .FirstOrDefault(c =>
                        {
                            var ps = c.GetParameters();
                            return ps.Length == 1 && ps[0].ParameterType.IsAssignableFrom(mappedProps.GetType());
                        });
                    if (preferredCtor != null)
                    {
                        implInstance = preferredCtor.Invoke(new[] { mappedProps });
                    }
                }

                // try parameterless ctor
                if (implInstance == null)
                {
                    var defaultCtor = implType.GetConstructor(Type.EmptyTypes);
                    if (defaultCtor != null)
                        implInstance = defaultCtor.Invoke(null);
                }

                // last resort: try first ctor with whatever we have
                if (implInstance == null)
                {
                    var anyCtor = implType.GetConstructors().FirstOrDefault();
                    if (anyCtor != null)
                    {
                        var parms = anyCtor.GetParameters();
                        var args = parms.Select(p => p.ParameterType.IsInstanceOfType(mappedProps) ? mappedProps : (p.HasDefaultValue ? p.DefaultValue : null)).ToArray();
                        try { implInstance = anyCtor.Invoke(args); } catch { implInstance = null; }
                    }
                }

                if (implInstance is TiledTypesUsed tiledImpl)
                {
                    return tiledImpl;
                }
                else
                {
                    Console.WriteLine($"implementation for \"{typeName}\" does not implement TiledTypesUsed");
                }
            }
            else
            {
                Console.WriteLine($"class \"{tileData.Type}\" not implemented");
            }
            return null;
        }

        private bool CreatePowerUpEntity(object source, Tileset tileset, Tile tileData)
        {
            if (!tileset.Image.HasValue)
                return false;

            Rectangle destRect;
            if (source is DotTiled.Object obj)
                destRect = GetObjectDestinationRectangle(obj);
            else if (source is Rectangle rect)
                destRect = rect;
            else
                return false;

            Entity entity = new Entity(TilemapTextures[tileset],
                GetSourceRect(tileData.ID, tileset),
                destRect,
                1,
                Color.White);

            PowerUpGiverComponent c = new();
            switch (tileData.Type)
            {
                case "DoubleJump":
                    var jumpData = (source is DotTiled.Object objSource) ?
                        objSource.MapPropertiesTo<DoubleJump>() ?? new DoubleJump() :
                        tileData.MapPropertiesTo<DoubleJump>();
                    c.componentGived = new DoubleJumpComponent(jumpData);
                    break;
                default:
                    Console.WriteLine($"PowerUp type not implemented: {tileData.Type}");
                    return false;
            }

            if (c.componentGived == null)
                return false;

            c.Start();
            entity.AddComponent(c.GetType(), c);
            entities.Add(entity);
            return true;
        }

        public void CreateEntities(ObjectLayer objectLayer)
        {
            foreach (var obj in objectLayer.Objects)
            {
                float x = obj.X / Map.TileWidth * TILESIZE;
                float y = obj.Y / Map.TileHeight * TILESIZE;
                EntityPositionerByName[obj.Type] = new Vector2(x, y);
                //TODO: make this work better, its a really bad implementation. Use TileObjects or something to add new players

                if (obj is not TileObject tile)
                {
                    if (obj.Type == "PlayerSpawner")
                    {
                        var playable = obj.MapPropertiesTo<PlayerSpawner>();
                        uint ra = (uint)new Random().Next(1, 10);
                        var tilesett = TilesetsByGID[ra];
                        var text2d = TilemapTextures[tilesett];
                        Entity entity = new(text2d, GetSourceRect(ra, tilesett), new((int)x, (int)y, TILESIZE, TILESIZE), 0.5f, Color.White);
                        entity.isPlayable = playable.isPlayable;
                        entity.isPlayer = playable.isPlayer;
                        entities.Add(entity);
                    } 
                continue;
                }
                var tileset = TilesetsByGID[tile.GID];
                var tileData = TilesByGID[tile.GID];
                if (tileData == null || tileset == null) continue;

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
                    Entity entity;
                    if(tileset.Image.HasValue)
                    {
                        Texture2D atlasImage = TilemapTextures[tileset];
                        entity = new Entity(atlasImage, GetSourceRect(tile.GID, tileset), GetObjectDestinationRectangle(tile), 1, Color.White);
                    }
                    else
                    {
                        Texture2D atlasImage = TileCollectionTextures[tileData];
                        entity = new Entity(atlasImage, TileSourceBounds(tileData), GetObjectDestinationRectangle(tile), 1, Color.White);
                    }

                    entity.AddComponents(new List<Component>
                    {
                        new NPCAnimationComponent(tileData),
                        new NPCComponent(papu.name, papu.dialogStart, papu.dialogEnd, gum)
                    });
                    entities.Add(entity);
                    Console.WriteLine($"added entiity {papu.name} with tileset {tileset.Name} in position {entity.Destinationrectangle}");
                }
                if (tile.Type == "DoubleJump" || tileData.Type == "DoubleJump")
                {
                    CreatePowerUpEntity(obj, tileset, tileData);
                }
            }
        }

        public void drawTileLayer(GameTime gameTime, SpriteBatch spriteBatch, TileLayer tileLayer, Camera camera)
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
                Tileset atlasImage = TilesetsByGID[value];
                if (atlasImage.Image.HasValue)
                {
                    DrawTile(gameTime, spriteBatch, value, position, atlasImage, camera);
                }
                else{
                    value++;
                    DrawCollectionTile(gameTime, spriteBatch, value, position, camera);
                }
            }
        }

        private void DrawCollectionTile(GameTime gameTime, SpriteBatch spriteBatch, uint value, Vector2 position, Camera camera)
        {
            var desRectangle = getDestinationRectangle(position, TilesetsByGID[value]);
            if (!IsVisible(desRectangle, camera)) return;
            Tile tile = TilesByGID[value];
            Texture2D text = TileCollectionTextures[tile];
            Rectangle src = TileSourceBounds(tile);
            spriteBatch.Draw(text, desRectangle, src, Color.White);
        }

        private void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, uint value, Vector2 position, Tileset atlasImage, Camera camera)
        {
            Rectangle desRectangle = getDestinationRectangle(position, atlasImage);
            if (!IsVisible(desRectangle, camera)) return;
            Texture2D texture = TilemapTextures[atlasImage];
            Rectangle srcRectangle = GetSourceRect(value, atlasImage);
            spriteBatch.Draw(texture, desRectangle, srcRectangle, Color.White);
        }

        private Rectangle getDestinationRectangle(Vector2 position, Tileset tileset)
        {
            return new((int)position.X * TILESIZE,
                        (int)((position.Y - (tileset.TileHeight / TileHeight)+1) * TILESIZE), //adding one because 8/8 = 1, 16/8=2, i just need that 2 because its the difference between a tileset with the same height and a tileset with another height 
                        (int)((tileset.TileWidth / TileWidth) * TILESIZE),
                        (int)((tileset.TileHeight / TileHeight) * TILESIZE));
        }

        public static Rectangle GetSourceRect(uint id, Tileset tileset)
        {
            uint col = id % tileset.Columns;
            uint row = id / tileset.Columns;

            uint x = tileset.Margin + (col * (tileset.TileWidth + tileset.Spacing));
            uint y = tileset.Margin + (row * (tileset.TileHeight + tileset.Spacing));

            return new Rectangle((int)x, (int)y, (int)tileset.TileWidth, (int)tileset.TileHeight);
        }
        public static Rectangle TileSourceBounds(Tile tile)
        {
            return new((int)tile.X, (int)tile.Y, (int)tile.Width, (int)tile.Height);
        }

        private void InitTilesets(List<Tileset> tilesets, string path)
        {
            foreach (Tileset tileset in tilesets)
            {
                if (tileset.Image.HasValue)
                {
                    TilemapTextures.Add(tileset, LoadImage(graphicsDevice, path, tileset.Image));
                }
                else
                {
                    foreach (var tile in tileset.Tiles)
                    {
                        TileCollectionTextures.Add(tile, LoadImage(graphicsDevice, path, tile.Image));
                    }
                }
            }

            for (uint gid = 1; gid < GID_Count_Last; gid++) 
            {
                var (tileset, tile) = GetTilesetFromGID(gid);

                TilesetsByGID.Add(gid, tileset);
                TilesByGID.Add(gid, tile);
            }
        }
        public (Tileset tileset, Tile tile) GetTilesetFromGID(uint gid, bool safe = false)
        {
            foreach (Tileset tileset in Map.Tilesets)
            {
                if (gid > tileset.TileCount + tileset.FirstGID && tileset.TileCount + tileset.FirstGID < GID_Count) // Im so sorry :(
                {
                    continue;
                }
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
                {
                    // throw new Exception($"No tileset with gid {gid} exists.");
                    Console.WriteLine($"no tileset with gid {gid} exist");
                    return (null, null);
                }
                else
                    throw new Exception($"gid {gid} doesn't have FlippingFlag bits stripped");
            }
            else
            {
                return (null, null);
            }
        }
        public void DrawLayerGroup(GameTime gameTime, SpriteBatch spriteBatch, List<BaseLayer> layers, Camera camera, List<Entity> entities)
        {
            foreach (BaseLayer layer in layers)
            {
                if (!layer.Visible) continue;
                switch (layer)
                {
                    case Group group:
                        DrawLayerGroup(gameTime, spriteBatch, group.Layers, camera, entities);
                        break;
                    case TileLayer tileLayer: // WARNING: this area can be animated very easily, but i dont add that because ALL ANIMATED TILES SHOULD BE INTERACTUABLE, if you see something that is moving, you should be able to interact with it
                        if (tileLayer.Class == "Collision Tile Layer")
                        {
                            DrawTileCollisionLayer(gameTime, spriteBatch, tileLayer, camera);
                            break;
                        }
                        drawTileLayer(gameTime, spriteBatch, tileLayer, camera);
                        break;
                    case ObjectLayer objectLayer:
                        if(!DrawEntities(gameTime, spriteBatch, entities, objectLayer))
                            continue;
                        DrawObjectLayer(gameTime, spriteBatch, objectLayer, camera);
                        break;
                    case ImageLayer imageLayer:
                        DrawImageLayer(gameTime, spriteBatch, imageLayer, camera);
                        break;
                    default:
                        Console.WriteLine($"layer type {layer.GetType()} not implemented");
                        break;
                }
            }
        }

        public bool DrawEntities(GameTime gameTime, SpriteBatch spriteBatch, List<Entity> entities, ObjectLayer objectLayer)
        {
            if (objectLayer.Class == "Entity Spawner")
            {
                foreach (var entity in entities)
                {
                    entity.Draw(gameTime, spriteBatch);
                }
                return false;
            }
            return true;
        }

        private void DrawImageLayer(GameTime gameTime, SpriteBatch spriteBatch, ImageLayer imageLayer, Camera camera)
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

        private void DrawTileCollisionLayer(GameTime gameTime, SpriteBatch spriteBatch, TileLayer tileLayer, Camera camera)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs; //get the csv
            for (int i = 0; i < data.Length; i++)
            {
                uint gid = data[i];
                if (gid == 0) continue;

                int x = (int)(i % tileLayer.Width);
                int y = (int)(i / tileLayer.Width);
                var pos = new Vector2(x, y);

                if (!collisionLayer.TryGetValue(pos, out var block) ||
                    !TilesByGID.TryGetValue(gid, out var tile) ||
                    !IsVisible(block.collider, camera))
                    continue;

                var atlas = TilesetsByGID[gid];
                if (atlas.Image.HasValue)
                    block.Draw(gameTime, spriteBatch, TilemapTextures[atlas], GetSourceRect(gid - atlas.FirstGID, atlas));
                else
                    block.Draw(gameTime, spriteBatch, TileCollectionTextures[tile], TileSourceBounds(tile));
            }
        }

        private void DrawObjectLayer(GameTime gameTime, SpriteBatch spriteBatch, ObjectLayer objectLayer, Camera camera)
        {
            if (!objectLayer.Visible) return;
            foreach (var obj in objectLayer.Objects)
            {
                switch (obj)
                {
                    case TileObject tileObject:
                        DrawTileObject(gameTime, spriteBatch, tileObject, objectLayer, camera);
                        break;
                    default:
                        // Console.WriteLine("My fault gang"); //TODO: create object Draw for debugging
                        break;
                }
            }
        }

        private void DrawTileObject(GameTime gameTime, SpriteBatch spriteBatch, TileObject tileObject, ObjectLayer objectLayer, Camera camera)
        {
            var destRect = GetObjectDestinationRectangle(tileObject);
            if (!IsVisible(destRect, camera)) return;

            var tileset = TilesetsByGID[tileObject.GID];
            var tile = TilesByGID[tileObject.GID];

            var texture = tileset.Image.HasValue ?
                        TilemapTextures[tileset] : TileCollectionTextures[tile];
            var sourceRectangle = tileset.Image.HasValue ?
                        GetSourceRect(tileObject.GID - tileset.FirstGID, tileset) : TileSourceBounds(tile);

            if (dynamicBlocks.TryGetValue(tileObject, out var dyn))
            {
                dyn.Draw(gameTime, spriteBatch, texture, sourceRectangle);
                return; 
            }

            var tileCoordinate = new Vector2((int)(tileObject.X / TileWidth), (int)(tileObject.Y / TileHeight) - (int)(tileObject.Height / TileHeight));
            if (collisionLayer.TryGetValue(tileCoordinate, out var col))
            {
                col.Draw(gameTime, spriteBatch, texture, sourceRectangle);
                return;
            }
            spriteBatch.Draw(texture, destRect, sourceRectangle, Color.White); //draws the object but without animation or interactivity.
        }
        private bool IsVisible(Rectangle destRect, Camera camera)
        {
            return camera.IsRectangleVisible(destRect);
        }

        private Rectangle GetObjectDestinationRectangle(DotTiled.Object obj)
        {
            return new Rectangle(
                            (int)(obj.X / TileWidth * TILESIZE),
                            (int)(((obj.Y / TileHeight) - (obj.Height / TileHeight)) * TILESIZE), //objects anchor are in the bottom left corner
                            (int)(obj.Width / TileWidth * TILESIZE),
                            (int)(obj.Height / TileHeight * TILESIZE));
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
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, List<Entity> entities)
        {
            DrawLayerGroup(gameTime, spriteBatch, Map.Layers, camera, entities);
        }
    }
}