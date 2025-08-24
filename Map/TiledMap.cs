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

namespace Juegazo.Map
{

    public class TiledMap
    {
        public DotTiled.Map Map { get; }
        public uint Width => Map.Width;
        public uint Height => Map.Height;
        public uint TileHeight => Map.TileHeight;
        public uint TileWidth => Map.TileWidth;

        public Dictionary<Vector2, Block> tileBlockLayer { get; } = new();
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public Dictionary<Vector2, Block> collisionLayer { get; } = new();
        public List<MovementBlock> dynamicBlocks;
        public readonly List<ICustomTypeDefinition> CustomTypeDefinitions = new();
        public TiledMap(string mapFilePath, List<ICustomTypeDefinition> typeDefinitions)
        {
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            var loader = Loader.DefaultWith(customTypeDefinitions: typeDefinitions);
            Map = loader.LoadMap(mapFilePath);
        }
        public TiledMap(string mapFilePath)
        {
            var loader = Loader.Default();
            Map = loader.LoadMap(mapFilePath);
            Console.WriteLine("shit compiles");
        }
        public void DrawLayerGroup(SpriteBatch spriteBatch, List<BaseLayer> layers, Vector2 viewOffset)
        {
            foreach (BaseLayer layer in layers)
            {
                Vector2 parallax = new Vector2(layer.ParallaxX, layer.ParallaxY);
                Vector2 offset = viewOffset * parallax;
                switch (layer)
                {
                    case Group group:
                        DrawLayerGroup(spriteBatch, group.Layers, new Vector2(group.OffsetX, group.OffsetY));
                        break;
                }
            }
        }
    }
}