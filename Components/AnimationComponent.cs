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
        private int frameWidth = 16;
        int frameHeight = 16;
        public Rectangle sourceRectangle { get; protected set; } = new();
        public AnimationComponent()
        {
            this.EnableDraw = true;
            this.EnableUpdate = true;
        }
        public AnimationComponent(int totalFrames, int currentFrame, int frameWidth, int FrameHeight)
        {
            this.totalFrames = totalFrames;
            this.currentFrame = currentFrame;
            this.frameWidth = frameWidth;
            this.frameHeight = FrameHeight;
            this.EnableDraw = true;
            this.EnableUpdate = true;
        }
        public override void Destroy()
        {
            Console.WriteLine("hay causita");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // if (Owner.hasComponent(typeof(NPCComponent))) Console.WriteLine("this owner has a component");
            var effects = !Owner.directionLeft ? SpriteEffects.None  : SpriteEffects.FlipHorizontally ;
            spriteBatch.Draw(Owner.texture, Owner.Destinationrectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, effects, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;
            if (frameCounter % 5 == 0)
            {
                if (Owner.velocity.X != 0)
                {
                    currentFrame = (currentFrame + 1) % totalFrames;
                }

                if (Owner.hasComponent(typeof(NPCComponent)))
                    currentFrame = (currentFrame + 1) % totalFrames;
            }
            sourceRectangle = new Rectangle(currentFrame * frameWidth, Owner.sourceRectangle.Y, frameWidth, frameHeight);
        }
    }
}