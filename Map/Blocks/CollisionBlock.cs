using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Juegazo.Components;

namespace Juegazo
{
    public class CollisionBlock : Block
    {
        public CollisionBlock(Rectangle collisionRectangle) : base(collisionRectangle)
        {
            value = 11;
        }
        public CollisionBlock() { value = 11; }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity is Player player) player.hasJumpedWall = false;

            int entityRight = entity.Destinationrectangle.Right;
            int entityLeft = entity.Destinationrectangle.Left;
            int blockRight = collision.Right;
            int blockLeft = collision.Left;

            if (entity.velocity.X > 0.0f || entityRight >= blockLeft && entityLeft < blockLeft)
            {
                entity.Destinationrectangle.X = blockLeft - entity.Destinationrectangle.Width;
            }
            else if (entity.velocity.X < 0.0f || entityLeft <= blockRight && entityRight > blockRight)
            {
                entity.Destinationrectangle.X = blockRight;
            }
            // If entity is clipping through, push them out
            else if (entityLeft < blockRight && entityRight > blockLeft)
            {
                float distanceToRight = Math.Abs(entityLeft - blockRight);
                float distanceToLeft = Math.Abs(entityRight - blockLeft);
                if (distanceToRight < distanceToLeft)
                {
                    entity.Destinationrectangle.X = blockRight;
                }
                else
                {
                    entity.Destinationrectangle.X = blockLeft - entity.Destinationrectangle.Width;
                }
            }
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            int entityBottom = entity.Destinationrectangle.Bottom;
            int entityTop = entity.Destinationrectangle.Top;
            int blockBottom = collision.Bottom;
            int blockTop = collision.Top;

            if (entity.velocity.Y > 0.0f || entityBottom >= blockTop && entityTop < blockTop)
            {
                entity.Destinationrectangle.Y = blockTop - entity.Destinationrectangle.Height;
                entity.velocity.Y = 0;
                entity.onGround = true;
                if (entity is Player player1) player1.jumpCounter = 0;
            }
            else if (entity.velocity.Y < 0.0f || entityTop <= blockBottom && entityBottom > blockBottom)
            {
                entity.Destinationrectangle.Y = blockBottom;
                entity.velocity.Y *= 0.1f;
            }
            // If entity is clipping through, push them out
            else if (entityTop < blockBottom && entityBottom > blockTop)
            {
                float distanceToBottom = Math.Abs(entityTop - blockBottom);
                float distanceToTop = Math.Abs(entityBottom - blockTop);
                if (distanceToBottom < distanceToTop)
                {
                    entity.Destinationrectangle.Y = blockBottom;
                }
                else
                {
                    entity.Destinationrectangle.Y = blockTop - entity.Destinationrectangle.Height;
                }
            }
        }
    }
}