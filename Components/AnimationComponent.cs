using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class AnimationComponent : Component
    {
        private int frameCounter = 0;
        private int currentFrame = 0;
        private int totalFrames = 2; // adjust based on your sprite sheet
        private int frameWidth = 16;  // adjust to your frame width
        private int frameHeight = 16; // adjust to your frame height
        private Rectangle sourceRectangle = new();
        public AnimationComponent()
        {
            this.Visible = true;
            this.Enable = true;
        }
        public override void Destroy()
        {
            Console.WriteLine("hay causita");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var effects = !Owner.directionLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Owner.texture, Owner.Destinationrectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, effects, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;
            if (frameCounter % 5 == 0)
            {
                if(Owner.velocity.X !=0 )
                {
                    currentFrame = (currentFrame + 1) % totalFrames;
                }
            }
            sourceRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        }
    }
}