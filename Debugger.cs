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
            rectangleTexture.SetData(new Color[] { new(255, 255, 255, 255) });
        }
        public Debugger(GraphicsDevice graphicsDevice, Color color)
        {
            this.graphicsDevice = graphicsDevice;
            rectangleTexture = new Texture2D(graphicsDevice, 1, 1);
            rectangleTexture.SetData(new Color[] { color });
        }
        public void drawhitboxEntities(SpriteBatch _spriteBatch, List<Entity> entities, HitboxTilemaps collisionMap, int TILESIZE)
        {
            foreach (var entity in entities)
            {
                foreach (var hitbox in collisionMap.getIntersectingTilesHorizontal(entity.Destinationrectangle))
                {
                    new Debugger(graphicsDevice).DrawRectHollow(_spriteBatch,
                        new Rectangle(
                        hitbox.X * TILESIZE,
                        hitbox.Y * TILESIZE,
                        TILESIZE,
                        TILESIZE
                    ), 4, Color.White);
                }
                foreach (var hitbox in collisionMap.getIntersectingTilesVertical(entity.Destinationrectangle))
                {
                    new Debugger(graphicsDevice).DrawRectHollow(
                        _spriteBatch,
                        new Rectangle(
                            hitbox.X * TILESIZE,
                            hitbox.Y * TILESIZE,
                            TILESIZE,
                            TILESIZE),
                        4, Color.White);
                }
            }
        }
        public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color)
        { //shows hitbox
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    rect.Width,
                    thickness
                ),
                color
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Bottom - thickness,
                    rect.Width,
                    thickness
                ),
                color
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                color
            );
            spriteBatch.Draw(
                    rectangleTexture,
                    new Rectangle(
                    rect.Right - thickness,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                color
            );
        }
    }
}