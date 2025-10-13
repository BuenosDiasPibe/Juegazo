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
        private float velocityToEntity { set; get; } = 1;
        private float lerpAmount = 0;
        public float velocity = 0;
        private Vector2 lastBlockPosition = new();
        private Vector2 newPosition = new();
        public bool movingLeft = false;

        public MovementBlock(Rectangle collider, Rectangle initialBlockPosition, Rectangle endBlockPosition, float velocity, bool canMove, Tile tile) : base(collider)
        {
            this.tile = tile;
            type = "MovementBlock";
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
            Vector2 current = new Vector2(endBlockPosition.X, endBlockPosition.Y);
            Vector2 target = new(initialBlockPosition.X, initialBlockPosition.Y);
            float distance = Vector2.Distance(current, target);
            float time = (float)(gameTime.TotalGameTime.TotalSeconds * velocity / distance);
            lerpAmount = 2 * Math.Abs(time % 1 - 0.5f);

            newPosition = Vector2.Lerp(current, target, lerpAmount);
            if (lerpAmount >= 0.99)
            {
                movingLeft = true;
            } if (lerpAmount <= 0.01)
            {
                movingLeft = false;
            }

            collider = new Rectangle((int)newPosition.X, (int)newPosition.Y, collider.Width, collider.Height);

            velocityToEntity = collider.X - lastBlockPosition.X;
            lastBlockPosition = new(collider.X, collider.Y);
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);
            // float f = 0.6f;

            entity.baseVelocity.X = velocityToEntity;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            base.Draw(gameTime, spriteBatch, texture, sourceRectangle);
        }
    }
}