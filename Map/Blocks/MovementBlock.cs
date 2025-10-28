using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DotTiled;
using Juegazo.Components;
using Juegazo.Map.Components;
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
            this.AddComponent(new MovementPingPongBlockComponent(initialBlockPosition, endBlockPosition, velocity));
            base.Start();
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //TODO: add horizontal actions
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var c in components.Where(c => c.EnableUpdate))
            {
                c.Update(gameTime);
                if(c is MovementPingPongBlockComponent ce)
                {
                    if (ce.lerpAmmount >= 0.99)
                    {
                        movingLeft = true;
                    } if (ce.lerpAmmount <= 0.01)
                    {
                        movingLeft = false;
                    }
                    velocityToEntity = ce.velocityToEntity;
                }
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);
            entity.baseVelocity = velocityToEntity;
        }
    }
}