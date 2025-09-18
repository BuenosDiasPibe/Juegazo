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
        private int totalFrames = 4; // adjust based on your sprite sheet
        private int frameWidth = 8;  // adjust to your frame width
        private int frameHeight = 8; // adjust to your frame height
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
            spriteBatch.Draw(Owner.texture, Owner.Destinationrectangle, sourceRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;
            if (frameCounter % 5 == 0)
            {
                Console.WriteLine("changed frame");
                currentFrame = (currentFrame + 1) % totalFrames;
                sourceRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            }
        }
    }
}