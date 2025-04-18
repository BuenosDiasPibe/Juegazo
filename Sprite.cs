using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public float scale;
        public Color color;
        public Rectangle rectangle
        {
            get
            {
                return new Rectangle(
                    (int)(position.X - texture.Width * scale / 2),
                    (int)(position.Y - texture.Height * scale / 2),
                    (int)(texture.Width * scale),
                    (int)(texture.Height * scale)
                );
            }
            set{rectangle =value;}
        }
        public Sprite(Texture2D texture, Vector2 position, float scale, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.color = color;
        }

        public void Update(GameTime gameTime)
        {
            // Update logic for the sprite can be added here
        }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
        }
    }
}