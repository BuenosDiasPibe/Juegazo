using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class Debugger
    {
        private Texture2D rectangleTexture;
        private GraphicsDevice graphicsDevice;
        public Debugger(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            rectangleTexture = new Texture2D(graphicsDevice, 1, 1);
            rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });
        }
        public void drawhitboxEntities(SpriteBatch _spriteBatch, List<Entity> entities, HitboxTilemaps collisionMap, int TILESIZE)
        {
            foreach (var entity in entities)
            {
                foreach (var hitbox in collisionMap.getIntersectingTilesHorizontal(entity.Destrectangle))
                {
                    new Debugger(graphicsDevice).DrawRectHollow(_spriteBatch,
                        new Rectangle(
                        hitbox.X * TILESIZE,
                        hitbox.Y * TILESIZE,
                        TILESIZE,
                        TILESIZE
                    ), 4);
                }
                foreach (var hitbox in collisionMap.getIntersectingTilesVertical(entity.Destrectangle))
                {
                    new Debugger(graphicsDevice).DrawRectHollow(
                        _spriteBatch,
                        new Rectangle(
                            hitbox.X * TILESIZE,
                            hitbox.Y * TILESIZE,
                            TILESIZE,
                            TILESIZE),
                        4);
                }
            }
        }
        public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness)
        { //shows hitbox
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Bottom - thickness,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
            spriteBatch.Draw(
                    rectangleTexture,
                    new Rectangle(
                    rect.Right - thickness,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
        }
    }
}