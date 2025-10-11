using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Components
{
    public class BlockAnimationComponent : BlockComponent
    {
        public override void Destroy()
        {
        }
        public override void Start()
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaa");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            double totalDuration = Owner.tile.Animation.Sum(a => a.Duration);
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds % totalDuration;
            int frame = 0;
            double accumulatedTime = 0;

            for (int i = 0; i < Owner.tile.Animation.Count; i++)
            {
                accumulatedTime += Owner.tile.Animation[i].Duration;
                if (currentTime <= accumulatedTime)
                {
                    frame = i;
                    break;
                }
            }
            var currentFrame = Owner.tile.Animation[frame];
            sourceRectangle.X = (int)(currentFrame.TileID % (texture.Width / sourceRectangle.Width) * sourceRectangle.Width);
            sourceRectangle.Y = (int)(currentFrame.TileID / (texture.Width / sourceRectangle.Width) * sourceRectangle.Height);

            spriteBatch.Draw(texture, Owner.collider, sourceRectangle, Microsoft.Xna.Framework.Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}