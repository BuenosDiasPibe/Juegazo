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
        public Rectangle initialBlockPosition = new();
        public Rectangle endBlockPosition = new();
        private Vector2 velocity = new();
        private float velocityToEntity { set; get; } = new();
        public MovementBlock(Rectangle collider, Rectangle initialBlockPosition, Rectangle endBlockPosition) : base(collider)
        {
            value = 16;
            velocity = checkMovementVector();
            velocityToEntity = velocity.Length() * 1.2f;
            EnableUpdate = true;
            this.initialBlockPosition = new(initialBlockPosition.X/initialBlockPosition.Width*32,
                                            (initialBlockPosition.Y/initialBlockPosition.Height*32)-1,
                                            32,
                                            32);
            this.endBlockPosition = new(endBlockPosition.X/endBlockPosition.Width*32,
                                        (endBlockPosition.Y/endBlockPosition.Height*32)-1,
                                        32,
                                        32);
        }
        public MovementBlock()
        {
            value = 16;
            EnableUpdate = true;
        }
        public MovementBlock(Rectangle collider) : base(collider){ value = 16; }
        

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
            if (collider.Intersects(initialBlockPosition))
                toEndPosition = false;
            else if (collider.Intersects(endBlockPosition))
                toEndPosition = true;

            Vector2 target = toEndPosition ? new Vector2(initialBlockPosition.X, initialBlockPosition.Y) : new Vector2(endBlockPosition.X, endBlockPosition.Y);

            Vector2 current = toEndPosition ? new Vector2(endBlockPosition.X, endBlockPosition.Y) : new Vector2(initialBlockPosition.X, initialBlockPosition.Y);
            float lerpAmount = (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 0.5 + 0.5) * 1f;
            Vector2 newPosition = new();

            if (toEndPosition)
                newPosition = Vector2.Lerp(current, target, lerpAmount);
            else
                newPosition = Vector2.Lerp(target, current, lerpAmount);

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