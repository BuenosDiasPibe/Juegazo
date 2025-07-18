using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class HitboxTilemaps : TileMaps
    {
        public List<BlockType> blocks;
        public HitboxTilemaps(Texture2D texture, int scaleTexture, int pixelSize, int numberOfTilesPerRow) : base(texture, scaleTexture, pixelSize, numberOfTilesPerRow)
        {
            tilemap = new();
            //pequeño hack para obtener todas las clases que hereden de BlockType
            blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(BlockType)) && !t.IsAbstract)
                .Select(t => (BlockType)Activator.CreateInstance(t))
                .ToList();
        }
        public WorldBlock createWorld(KeyValuePair<Vector2, int> item)
        {
            // Select the appropriate BlockType from the blocks list based on the item's value //GRACIAS COPILOT!!!
            BlockType block = blocks.FirstOrDefault(b => b.value == item.Value) ?? blocks.First();

            return new WorldBlock(texture, BuildSourceRectangle(item), BuildDestinationRectangle(item), Color.White, block);
        }

        //make a list of rectangles that the player is interacting with. The rectangles are created in WORLD coordinates (if player is between 2 blocks, create the rectangles of those blocks)
        public List<Rectangle> getIntersectingTilesVertical(Rectangle target)
        {
            List<Rectangle> intersection = new();

            int widthInTiles = (target.Width - (target.Width % scaleTexture)) / scaleTexture;
            int heightInTiles = (target.Height - (target.Height % scaleTexture)) / scaleTexture;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersection.Add(new Rectangle(
                        (target.X + x * (scaleTexture - 1)) / scaleTexture,
                        (target.Y + y * scaleTexture) / scaleTexture,
                        scaleTexture,
                        scaleTexture
                    ));
                }
            }
            return intersection;
        }
        public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target)
        {
            List<Rectangle> intersection = new();

            int widthInTiles = (target.Width - (target.Width % scaleTexture)) / scaleTexture;
            int heightInTiles = (target.Height - (target.Height % scaleTexture)) / scaleTexture;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersection.Add(new Rectangle(
                        (target.X + x * scaleTexture) / scaleTexture,
                        (target.Y + y * (scaleTexture - 1)) / scaleTexture,
                        scaleTexture,
                        scaleTexture
                    ));
                }
            }
            return intersection;
        }

    }
}