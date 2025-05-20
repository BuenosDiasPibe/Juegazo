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
        public HitboxTilemaps(Texture2D texture, int scaleTexture, int pixelSize) : base(texture, scaleTexture, pixelSize)
        {
            nombreTile = "hitbox";
            tilemap = new();
            sourceRectangles = new();
            destinationRectangles = new();
            intersections = new();
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

                    if (player.velocity.X > 0.0f)
                    {
                        player.Destrectangle.X = collision.Left - player.Destrectangle.Width;
                        // todo el resto son cosas que deberian ser manejadas por otra clase
                        player.velocity.Y *= 0.7f;
                        player.onGround = true; //puede saltar por las paredes
                        if (player.jumpPressed) player.pushBack = -20; //TODO: fixear esto haciendo que solo afecte cuando el jugador apreta el botn de saltar (osea lo que deberia pasar pero soy idiota :3)
                    }
                    else if (player.velocity.X < 0.0f)
                    {
                        player.Destrectangle.X = collision.Right;
                        player.onGround = true; //puede saltar por las paredes
                        player.velocity.Y *= 0.7f;
                        if (player.jumpPressed) player.pushBack = 20;
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

                    if (player.velocity.Y > 0.0f)
                    {
                        player.Destrectangle.Y = collision.Top - player.Destrectangle.Height;
                        player.velocity.Y = 1f;
                        player.onGround = true;
                    }
                    else if (player.velocity.Y < 0.0f)
                    {
                        player.velocity.Y *= 0.1f;
                        player.Destrectangle.Y = collision.Bottom;
                    }
                    player.numJumps = 0;
                }
            }
        }
    }
}