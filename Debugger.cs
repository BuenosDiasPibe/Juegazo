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
        public Debugger(GraphicsDevice graphicsDevice) {
            rectangleTexture = new Texture2D(graphicsDevice,1,1);
            rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });
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