using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Microsoft.Xna.Framework;

namespace Juegazo.Map
{
    public static class TiledMapUtilities
    {
        
        public static Rectangle getDestinationRectangle(Vector2 position, Tileset tileset, int TILESIZE, uint TileWidth, uint TileHeight)
        {
            return new((int)position.X * TILESIZE,
                        (int)((position.Y - (tileset.TileHeight / TileHeight)+1) * TILESIZE), //adding one because 8/8 = 1, 16/8=2, i just need that 2 because its the difference between a tileset with the same height and a tileset with another height 
                        (int)(tileset.TileWidth / TileWidth * TILESIZE),
                        (int)(tileset.TileHeight / TileHeight * TILESIZE));
        }
    }
}