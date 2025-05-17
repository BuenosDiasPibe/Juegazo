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
        public Color color;
        public Rectangle Destrectangle;
        public Rectangle sourceRectangle;
        public Sprite(Texture2D texture,Rectangle sourceRectangle, Rectangle Destrectangle, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            this.
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