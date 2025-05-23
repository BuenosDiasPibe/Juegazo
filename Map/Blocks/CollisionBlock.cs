using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Juegazo
{
    public class CollisionBlock : BlockType
    {
        public CollisionBlock()
        {
            value = 11;
        }
        public override void horizontalActions(Entity entity, Rectangle collision, int _val)
        {
            if (entity.velocity.X > 0.0f && _val == value)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
            }
            else if (entity.velocity.X < 0.0f && _val == value)
            {
                entity.Destinationrectangle.X = collision.Right;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision, int _val)
        {
            if (entity.velocity.Y > 0.0f && _val == value)
            {
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
            }
            else if (entity.velocity.Y < 0.0f && _val == value)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}