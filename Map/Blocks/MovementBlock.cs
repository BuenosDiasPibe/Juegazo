using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DotTiled;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class MovementBlock : Block
    {
        public bool movingLeft = false;
        public float velocity = 0;
        public Rectangle initialBlockPosition = new();
        public Rectangle endBlockPosition = new();
        private Vector2 velocityToEntity = new();
        private Vector2 lastBlockPosition = new();
        private Vector2 newPosition = new();

        public MovementBlock(Rectangle collider, Rectangle initialBlockPosition, Rectangle endBlockPosition, CustomTiledTypes.MovementBlock block) : base(collider)
        {
            EnableUpdate = block.canMove;
            this.initialBlockPosition = initialBlockPosition;
            this.endBlockPosition = endBlockPosition;
            this.velocity = block.velocity;
        }
        public MovementBlock()
        {
            EnableUpdate = false;
        }
        public MovementBlock(Rectangle collider) : base(collider) { }
        public override void Start()
        {
            if (initialBlockPosition.IsEmpty || endBlockPosition.IsEmpty || collider.IsEmpty)
            {
                throw new InvalidOperationException("MovementBlock not initialized: initialBlockPosition, encPositionBlock or collider is empty. Blocks with object references should not be added in tile layers");
            }
            base.Start();
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
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
            entity.baseVelocity = velocityToEntity;
        }
    }
}