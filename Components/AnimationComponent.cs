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
            var src = Owner.sourceRectangle;
            SpriteEffects effects = SpriteEffects.None;
            float rotation = 0f;
            var origin = new Vector2(0, 0);

            switch (Owner.direction)
            {
                case FACES.BOTTOM:
                    rotation = 0f;
                    break;
                case FACES.TOP:
                    rotation = 0f;
                    effects |= SpriteEffects.FlipVertically;
                    break;
                case FACES.LEFT:
                    rotation = MathHelper.PiOver2;
                    origin = new(0, Owner.sourceRectangle.Height);
                    break;
                case FACES.RIGHT:
                    rotation = -MathHelper.PiOver2;
                    origin = new(Owner.sourceRectangle.Width,0);
                    break;
                default:
                    break;
            }

            if (Owner.directionLeft)
            {
                effects |= SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(Owner.texture, Owner.Destinationrectangle, src, Owner.color, rotation, origin, effects, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (frameCounter >= 83) // 83ms is roughly equivalent to 12 frames per second
            {
            if (Owner.velocity.X != 0)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
            }

            if (Owner.hasComponent(typeof(NPCComponent)))
                currentFrame = (currentFrame + 1) % totalFrames;
                
            frameCounter = 0;
            }
            Owner.sourceRectangle = new Rectangle(currentFrame * frameWidth, Owner.sourceRectangle.Y, frameWidth, frameHeight);
        }
    }
}