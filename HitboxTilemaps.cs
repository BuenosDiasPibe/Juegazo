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
        protected int tileValue;
        public List<BlockType> blocks;
        public HitboxTilemaps(Texture2D texture, int scaleTexture, int pixelSize) : base(texture, scaleTexture, pixelSize)
        {
            tileValue = 11;
            nombreTile = "hitbox";
            tilemap = new();
            sourceRectangles = new();
            destinationRectangles = new();
            intersections = new();
            blocks = new();
            blocks.Add(new CollisionBlock());
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

        public void Update(Player player, int TILESIZE)
        {
            player.Destrectangle.X += (int)player.velocity.X;
            intersections = getIntersectingTilesHorizontal(player.Destrectangle);
            player.onGround = false;

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

                    if (!player.Destrectangle.Intersects(collision)) continue;

                    foreach (var block in blocks)
                    {
                        block.horizontalActions(player, collision, _val);
                    }
                }
            }

            player.Destrectangle.Y += (int)player.velocity.Y;

            intersections = getIntersectingTilesVertical(player.Destrectangle);

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

                    if (!player.Destrectangle.Intersects(collision)) continue;
                    
                    foreach (var block in blocks)
                    {
                        block.verticalActions(player, collision, _val);
                    }
                }
            }
        }
    }
}