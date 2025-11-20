using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarinMol
{
    public class Debugger
    {
      //singleton ---
        private static Debugger instance;
        public static Debugger Instance {
          get {
            if(instance == null)
              throw new InvalidOperationException("Debugger was not initialized");
            return instance;
          }
        }
        private static readonly object sync = new();
        public static void Initialize(GraphicsDevice graphicsDevice, Color color)
        {
          if(instance != null) return;
          lock(sync)
          {
            if (instance == null)
              instance = new Debugger(graphicsDevice, color);
          }
        }
        // ---
        private readonly Texture2D rectangleTexture;
        private readonly GraphicsDevice graphicsDevice;

        private Debugger(GraphicsDevice graphicsDevice, Color color)
        {
            this.graphicsDevice = graphicsDevice;
            rectangleTexture = new Texture2D(graphicsDevice, 1, 1);
            rectangleTexture.SetData([color]);
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
