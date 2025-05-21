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
        private List<Rectangle> intersections;
        public List<BlockType> blocks;
        /// <summary>
        /// Initializes a new instance of the <see cref="HitboxTilemaps"/> class.
        /// </summary>
        /// <param name="texture">The texture to be used for the tilemap.</param>
        /// <param name="scaleTexture">The scale factor to apply to the texture.</param>
        /// <param name="pixelSize">The size of each pixel in the tilemap.</param>
        /// <remarks>
        /// Sets up the hitbox tilemap, initializes collections, and dynamically discovers all non-abstract subclasses of <see cref="BlockType"/>.
        /// </remarks>
        public HitboxTilemaps(Texture2D texture, int scaleTexture, int pixelSize) : base(texture, scaleTexture, pixelSize)
        {
            nombreTile = "hitbox";
            tilemap = new();
            sourceRectangles = new();
            destinationRectangles = new();
            intersections = new();
            //pequeño hack para obtener todas las clases que hereden de BlockType
            blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(BlockType)) && !t.IsAbstract)
                .Select(t => (BlockType)Activator.CreateInstance(t))
                .ToList();
        }

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

        public void Update(Entity entity, int TILESIZE)
        {
            entity.Destrectangle.X += (int)entity.velocity.X;
            intersections = getIntersectingTilesHorizontal(entity.Destrectangle);
            entity.onGround = false;

            foreach (var rect in intersections)
            {
                if (tilemap.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    Rectangle collision = new(
                        rect.X * TILESIZE,
                        rect.Y * TILESIZE,
                        TILESIZE,
                        TILESIZE
                    );

                    if (!entity.Destrectangle.Intersects(collision)) continue;

                    foreach (var block in blocks)
                    {
                        block.horizontalActions(entity, collision, _val);
                    }
                }
            }

            entity.Destrectangle.Y += (int)entity.velocity.Y;

            intersections = getIntersectingTilesVertical(entity.Destrectangle);

            foreach (var rect in intersections)
            {

                if (tilemap.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    Rectangle collision = new Rectangle(
                        rect.X * TILESIZE,
                        rect.Y * TILESIZE,
                        TILESIZE,
                        TILESIZE
                    );

                    if (!entity.Destrectangle.Intersects(collision)) continue;
                    
                    foreach (var block in blocks)
                    {
                        block.verticalActions(entity, collision, _val);
                    }
                }
            }
        }
    }
}