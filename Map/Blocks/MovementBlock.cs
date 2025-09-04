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
        private float velocityToEntity { set; get; } = 1;
        private float lerpAmount = 0;
        public float velocity = 0;
        private Vector2 lastPlayerPosition = new();
        private Vector2 newPosition = new();
        public bool movingLeft = false;

        public MovementBlock(Rectangle collider, Rectangle initialBlockPosition, Rectangle endBlockPosition, float velocity, bool canMove) : base(collider)
        {
            value = 16; ;
            EnableUpdate = true;
            this.initialBlockPosition = new(initialBlockPosition.X / initialBlockPosition.Width * 32,
                                            (initialBlockPosition.Y / initialBlockPosition.Height * 32) - 1,
                                            32,
                                            32);
            this.endBlockPosition = new(endBlockPosition.X / endBlockPosition.Width * 32,
                                        (endBlockPosition.Y / endBlockPosition.Height * 32) - 1,
                                        32,
                                        32);
            this.velocity = velocity;
            EnableUpdate = canMove;
            Console.WriteLine($"MovementBlock collider {collider}\ndirection {direction()}");
        }
        public MovementBlock()
        {
            value = 16;
            EnableUpdate = true;
        }
        public MovementBlock(Rectangle collider) : base(collider) { value = 16; }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //new CollisionBlock().horizontalActions(entity, collision);
            // int direction = toEndPosition ? -1 : 1;
            // if (entity.velocity.X > 0.0f)
            // {
            //     entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
            //     if (entity.velocity.X > 10) entity.velocity.X *= 0.1f;
            // }
            // else if (entity.velocity.X < 0.0f)
            // {
            //     if (entity.velocity.X < -10) entity.velocity.X *= 0.1f;
            //     entity.Destinationrectangle.X = collision.Right;
            // }
            // else if (entity.Destinationrectangle.Right > collision.Right) entity.velocity.X += velocityToEntity * direction;
            // else if (entity.Destinationrectangle.Left < collision.Left) entity.velocity.X -= velocityToEntity * direction; //those two lines cause the game to fucking die????
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 current = new Vector2(endBlockPosition.X, endBlockPosition.Y);
            Vector2 target = new(initialBlockPosition.X, initialBlockPosition.Y);
            float time = (float)(gameTime.TotalGameTime.TotalSeconds * velocity);
            lerpAmount = 2 * Math.Abs(time % 1 - 0.5f);

            newPosition = Vector2.Lerp(current, target, lerpAmount);
            if (lerpAmount >= 0.99)
            {
                movingLeft = true;
            }if (lerpAmount <= 0.01)
            {
                movingLeft = false;
            }

            collider = new Rectangle((int)newPosition.X, (int)newPosition.Y, collider.Width, collider.Height);
            velocityToEntity = newPosition.X - lastPlayerPosition.X;
            lastPlayerPosition = newPosition;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);

            float f = movingLeft ? 2 : -2;
            Console.WriteLine($"velocity given to player: {velocityToEntity+f}");
            entity.velocity.X = velocityToEntity + f;
        }
        public Vector2 direction()
        {
            return Vector2.Normalize(new Vector2(
                endBlockPosition.X - initialBlockPosition.X,
                endBlockPosition.Y - initialBlockPosition.Y
            ));
        }
    }
}