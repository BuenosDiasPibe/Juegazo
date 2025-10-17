using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class MovementBlock : Block
    {
        public Rectangle initialBlockPosition = new();
        public Rectangle endBlockPosition = new();
        private Vector2 velocityToEntity = new();
        public float velocity = 0;
        private Vector2 lastBlockPosition = new();
        private Vector2 newPosition = new();
        public bool movingLeft = false;

        public MovementBlock(Rectangle collider, Rectangle initialBlockPosition, Rectangle endBlockPosition, float velocity, bool canMove) : base(collider)
        {
            type = "MovementBlock";
            EnableUpdate = true;
            this.initialBlockPosition = initialBlockPosition;
            this.endBlockPosition = endBlockPosition;
            this.velocity = velocity;
            EnableUpdate = canMove;
        }
        public MovementBlock()
        {
            EnableUpdate = true;
        }
        public MovementBlock(Rectangle collider) : base(collider) { type = "MovementBlock"; }
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
            Vector2 current = initialBlockPosition.Location.ToVector2();
            Vector2 target = endBlockPosition.Location.ToVector2();
            float distance = Vector2.Distance(current, target);
            float time = (float)(gameTime.TotalGameTime.TotalSeconds * velocity / distance);
            float lerpAmount = 2 * Math.Abs(time % 1 - 0.5f);

            newPosition = Vector2.Lerp(current, target, lerpAmount);
            if (lerpAmount >= 0.99)
            {
                movingLeft = true;
            } if (lerpAmount <= 0.01)
            {
                movingLeft = false;
            }

            collider.Location = newPosition.ToPoint();

            velocityToEntity = collider.Location.ToVector2() - lastBlockPosition;
            lastBlockPosition = new(collider.X, collider.Y);
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);
            // float f = 0.6f;

            entity.baseVelocity = velocityToEntity;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            base.Draw(gameTime, spriteBatch, texture, sourceRectangle);
        }
    }
}