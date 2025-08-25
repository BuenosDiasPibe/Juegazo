using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using DotTiled.Serialization;
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
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public List<MovementBlock> dynamicBlocks;

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
            //var tileset = GetTilesetFromGID(gid).tileset;
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
        //testing
        private List<BaseLayer> layers = new();

        public TiledMap(GraphicsDevice graphicsDevice, string projectDirectory, string mapFilePath, List<ICustomTypeDefinition> typeDefinitions)
        {
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            var loader = Loader.DefaultWith(customTypeDefinitions: typeDefinitions);
            Map = loader.LoadMap(mapFilePath);
            layers = Map.Layers;
        }
        public TiledMap(GraphicsDevice graphicsDevice, string projectDirectory, string mapFilePath, int TILESIZE)
        {
            this.graphicsDevice = graphicsDevice;
            TiledProjectDirectory = projectDirectory;
            MapFilePath = mapFilePath;

            var loader = Loader.Default();
            Map = loader.LoadMap(Path.Combine(TiledProjectDirectory, MapFilePath));
            layers = Map.Layers;
            this.TILESIZE = TILESIZE;
            TestingStuff();
            InitTilesets(Map.Tilesets, TiledProjectDirectory);
        }
        public void TestingStuff()
        {
            foreach (var layer in layers)
            {
                Console.WriteLine($"layername {layer.Name}");
                switch (layer)
                {
                    case TileLayer tilelayer:
                        Console.WriteLine("layerData: ");
                        Console.WriteLine(tilelayer.Data.Value.GlobalTileIDs);
                        break;
                    case ObjectLayer objectLayer:
                        Console.WriteLine("objectLayer: ");
                        break;
                }
            }
        }
        public void drawTileLayer(SpriteBatch spriteBatch, TileLayer tileLayer)
        {
            uint[] data = tileLayer.Data.Value.GlobalTileIDs; //get the csv
            for (int i = 0; i < data.Length; i++)
            {
                uint value = data[i];
                if (value == 0) continue; //air data
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

        private void DrawTile(SpriteBatch spriteBatch, uint value, Vector2 position, Tileset atlasImage)
        {
            Texture2D texture = TilemapTextures[atlasImage];
            //Console.WriteLine($"value={value-1}, x={value%8}, y={value/8}");
            int x = (int)((value-1) % 8); //TODO: change to tiles per row in the atlas image (its not saved in tiled by default fuck you)
            int y = (int)((value-1) / 8);
            Rectangle srcRectangle = new(
                (int)(x * atlasImage.TileWidth),
                (int)(y * atlasImage.TileHeight),
                (int)atlasImage.TileWidth,
                (int)atlasImage.TileHeight
            );
            Rectangle desRectangle = new((int)position.X * TILESIZE, (int)position.Y * TILESIZE, TILESIZE, TILESIZE);
            spriteBatch.Draw(texture, desRectangle, srcRectangle, Color.White);
        }

        private void InitTilesets(List<Tileset> tilesets, string path)
        {
            foreach (Tileset tileset in tilesets)
            {
                TilesetsByName.Add(tileset.Name, tileset);
                if (tileset.Image.HasValue)
                {
                    Image image = tileset.Image;
                    string tileset_path = tileset.Source;
                    TilemapTextures.Add(tileset, LoadImage(graphicsDevice, path, tileset.Image));
                }
            }

            //hit every number to add nulls to dictionaries
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
                Vector2 parallax = new Vector2(layer.ParallaxX, layer.ParallaxY);
                switch (layer)
                {
                    case Group group:
                        DrawLayerGroup(spriteBatch, group.Layers);
                        break;
                    case TileLayer tileLayer:
                        drawTileLayer(spriteBatch, tileLayer);
                        break;
                }
            }
        }
        public static Texture2D LoadImage(GraphicsDevice graphicsDevice, string caller_directory, Image image)
        {
            string relative_path = image.Source;
            string file = Path.GetFileName(relative_path);
            string folder = Path.GetDirectoryName(relative_path);
            string path = Path.Combine(caller_directory, folder, file);

            //TODO: prevent redundant texture reloading. Slowest step of reload by far.
            return LoadImage(graphicsDevice, path, caller_directory);
        }
        public static Texture2D LoadImage(GraphicsDevice graphicsDevice, string path, string projectDirectory)
        {
            path = Path.Combine(projectDirectory, path);
            return Texture2D.FromFile(graphicsDevice, path, DefaultColorProcessors.PremultiplyAlpha);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawLayerGroup(spriteBatch, layers);
        }
    }
}