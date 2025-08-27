using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class MovementBlock : Block
    {
        private bool toEndPosition = false;
        private Rectangle initialBlockPosition = new();
        private Rectangle endBlockPosition = new();
        private Vector2 velocity = new();
        private float velocityToEntity { set; get; } = new();
        public Rectangle DestRectangle { get; protected set; } = new();
        public MovementBlock(Rectangle DestinationRectangle, Rectangle collider, Rectangle initialBlockPosition, Rectangle endBlockPosition) : base(collider)
        {
            value = 16;
            velocity = checkMovementVector();
            velocityToEntity = velocity.Length() * 1.2f;
            EnableUpdate = true;
            this.DestRectangle = DestinationRectangle;
            this.initialBlockPosition = initialBlockPosition;
            this.endBlockPosition = endBlockPosition;
        }
        public MovementBlock()
        {
            value = 16;
            EnableUpdate = true;
        }
        

        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (velocity == new Vector2(0, 0))
            {
                velocity = checkMovementVector();
                velocityToEntity = velocity.Length() * 1.2f;
            }
            int direction = toEndPosition ? -1 : 1;
            if (entity.velocity.X > 0.0f)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                if (entity.velocity.X > 10) entity.velocity.X *= 0.1f;
            }
            else if (entity.velocity.X < 0.0f)
            {
                if (entity.velocity.X < -10) entity.velocity.X *= 0.1f;
                entity.Destinationrectangle.X = collision.Right;
            }
            else if (entity.Destinationrectangle.Right > collision.Right) entity.velocity.X += velocityToEntity * direction;
            else if (entity.Destinationrectangle.Left < collision.Left) entity.velocity.X -= velocityToEntity * direction;
        }
        private Vector2 checkMovementVector()
        {
            Vector2 direction = new Vector2(
                endBlockPosition.X - initialBlockPosition.X,
                endBlockPosition.Y - initialBlockPosition.Y
            );
            direction.Normalize();
            return direction;
        }

        public override void Update(GameTime gameTime)
        {
            if (DestRectangle.Intersects(initialBlockPosition))
            {
                toEndPosition = false;
            }
            else if (DestRectangle.Intersects(endBlockPosition))
            {
                toEndPosition = true;
            }

            Vector2 target = toEndPosition ? new Vector2(initialBlockPosition.X, initialBlockPosition.Y) : new Vector2(endBlockPosition.X, endBlockPosition.Y);

            Vector2 current = new Vector2(DestRectangle.X, DestRectangle.Y);

            Vector2 newPosition = Vector2.Lerp(current, target, 0.02f);

            DestRectangle = new Rectangle((int)newPosition.X, (int)newPosition.Y, DestRectangle.Width,DestRectangle.Height);

            collider = new Rectangle((int)newPosition.X, (int)newPosition.Y, collider.Width, collider.Height);
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.Y > 0.0f)
            {
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                if (toEndPosition)
                {
                    entity.velocity.X += velocityToEntity - entity.velocity.X;
                }
                else
                {
                    entity.velocity.X += -velocityToEntity + Math.Abs(entity.velocity.X);
                }
                entity.velocity.Y = 1f;
                entity.onGround = true;
                if (entity.hasComponent(typeof(MoveVerticalComponent)))
                {
                    ((MoveVerticalComponent)entity.getComponent(typeof(MoveVerticalComponent))).jumpCounter = 0;
                }
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}