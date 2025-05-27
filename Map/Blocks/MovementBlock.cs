using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class MovementBlock : BlockType
    {
        private bool moveRight;
        private Rectangle rightBound;
        private Rectangle leftBound;
        public MovementBlock()
        {
            value = 16;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.X > 0.0f)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
            }
            else if (entity.velocity.X < 0.0f)
            {
                entity.Destinationrectangle.X = collision.Right;
            }
        }
        public override void Update() { }

        public void Update(WorldBlock worldBlock)
        {
            rightBound = new Rectangle(worldBlock.Destinationrectangle.Width * 18, worldBlock.Destinationrectangle.Y, worldBlock.Destinationrectangle.Width, worldBlock.Destinationrectangle.Height);

            leftBound = new Rectangle(worldBlock.Destinationrectangle.Width, worldBlock.Destinationrectangle.Y, worldBlock.Destinationrectangle.Width, worldBlock.Destinationrectangle.Height);
            if (worldBlock.Destinationrectangle.Intersects(rightBound))
            {
                moveRight = false;
            }
            else if (worldBlock.Destinationrectangle.Intersects(leftBound))
            {
                moveRight = true;
            }

            int direction = moveRight ? 1 : -1;
            worldBlock.Destinationrectangle.X += 5 * direction;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.Y > 0.0f)
            {
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                entity.Destinationrectangle.X += moveRight ? 5 : -5; //little cheat, maybe change it with velocity rather than interacting with rectangle, but it works!
                entity.velocity.Y = 1f;
                entity.onGround = true;
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}