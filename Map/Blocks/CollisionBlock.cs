using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Juegazo.Components;

namespace Juegazo.Map.Blocks
{
    public class CollisionBlock : Block
    {
        public CollisionBlock() { }
        public CollisionBlock(Rectangle collision) : base(collision) { }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.baseVelocity = new();
            int entityRight = entity.Destinationrectangle.Right;
            int entityLeft = entity.Destinationrectangle.Left;
            int blockRight = collision.Right;
            int blockLeft = collision.Left;

            if (entityRight >= blockLeft && entityLeft < blockLeft)
            {
                entity.Destinationrectangle.X = blockLeft - entity.Destinationrectangle.Width;
            }
            else if (entityLeft <= blockRight && entityRight > blockRight)
            {
                entity.Destinationrectangle.X = blockRight;
            }
            if (entity.direction == FACES.LEFT || entity.direction == FACES.RIGHT)
            {
                entity.onGround = true;
                entity.velocity.Y = 0;
            }
        }


        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.baseVelocity = new();
            int entityBottom = entity.Destinationrectangle.Bottom;
            int entityTop = entity.Destinationrectangle.Top;
            int blockBottom = collision.Bottom;
            int blockTop = collision.Top;
            if (entityBottom >= blockTop && entityTop < blockTop)
            {
                entity.Destinationrectangle.Y = blockTop - entity.Destinationrectangle.Height;
            }
            else if(entityTop <= blockBottom && entityBottom > blockTop)
            {
                entity.Destinationrectangle.Y = blockBottom;
            }
            if (entity.direction == FACES.TOP || entity.direction == FACES.BOTTOM)
            {
                entity.onGround = true;
                entity.velocity.Y = 0;
            }
        }
    }
}