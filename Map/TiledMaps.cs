using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using DotTiled.Serialization;
using Juegazo.Map.Blocks;
using Microsoft.Xna.Framework;
using ToolsUtilities;

namespace Juegazo.Map
{

    public class DotTiledImplementation
    {
        public DotTiled.Map Map { get; }
        public string TiledProjectDirectory { get; }
        public string MapFilePath { get; }
        public string MapFileDirectory => Path.Combine(TiledProjectDirectory, Path.GetDirectoryName(MapFileDirectory));
        public uint Width => Map.Width;
        public uint Height => Map.Height;
        public uint TileHeight => Map.TileHeight;
        public uint TileWidth => Map.TileWidth;

        public Dictionary<Vector2, Block> tileBlockLayer { get; } = new();
        public Dictionary<string, BaseLayer> AllLayersByName { get; } = new();
        public Dictionary<Vector2, Block> collisionLayer { get; } = new();
        public List<MovementBlock> dynamicBlocks;
        public readonly List<ICustomTypeDefinition> CustomTypeDefinitions = new();
        public DotTiledImplementation(string projectDirectory, string mapFilePath, List<ICustomTypeDefinition> typeDefinitions)
        {
            MapFilePath = mapFilePath;
            TiledProjectDirectory = projectDirectory;
            foreach (var t in typeDefinitions)
            {
                CustomTypeDefinitions.Add(t);
            }
            var loader = Loader.DefaultWith(customTypeDefinitions: typeDefinitions);
            Map = loader.LoadMap(Path.Combine(TiledProjectDirectory, MapFilePath));
        }
        public DotTiledImplementation(string projectDirectory, string mapFilePath)
        {
            Console.WriteLine("shit compiles");
        }
    }
}