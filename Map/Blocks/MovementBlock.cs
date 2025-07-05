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
        private int velocity;
        private float velocityToEntity;
        public MovementBlock()
        {
            value = 16;
            velocity = 5;
            velocityToEntity = velocity * 1.2f;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.horizontalBlockMovementAction = true;
            if (entity.velocity.X > 0.0f)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                if (entity.velocity.X > 20) entity.velocity.X *= 0.1f;
            }
            else if (entity.velocity.X < 0.0f)
            {
                if (entity.velocity.X < -20)
                {
                    entity.velocity.X *= 0.1f;
                }
                entity.Destinationrectangle.X = collision.Right;
            }
            else if (entity.Destinationrectangle.Right > collision.Right)
            {
                entity.velocity.X += velocityToEntity;
            }
            else if (entity.Destinationrectangle.Left < collision.Left)
            {
                entity.velocity.X -= velocityToEntity;
            }
        }

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
            worldBlock.Destinationrectangle.X += velocity * direction; //FIXME: velocity and velocityToEntity if same they dont add the same velocity to the other, so enity is slightly slower than the movementBlock
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.Y > 0.0f)
            {
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                if (moveRight)
                {
                    entity.velocity.X += velocityToEntity - entity.velocity.X;
                }
                else
                {
                    entity.velocity.X += -velocityToEntity + Math.Abs(entity.velocity.X);
                }
                entity.velocity.Y = 1f;
                entity.onGround = true;
                entity.horizontalBlockMovementAction = false;
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
                entity.horizontalBlockMovementAction = false;
            }
        }
    }
}