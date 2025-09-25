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
        public Rectangle Destinationrectangle = new();
        public Rectangle sourceRectangle = new();
        public Sprite(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color)
        {
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
            this.Destinationrectangle = Destrectangle;
            this.color = color;
        }


        public void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Destinationrectangle, sourceRectangle, color);
        }
    }
}