using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Juegazo.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class JumpWallBlock : Block
    {
        private float jumpStrength = 11;
        public bool canJump = true;
        private float recoilVelocity = 10;
        public JumpWallBlock(Rectangle collider) : base(collider) { }
        public JumpWallBlock(Rectangle collider, int jumpSptrength, bool canJump, float recoilVelocity) : base(collider)
        {
            jumpStrength = jumpSptrength;
            this.recoilVelocity = recoilVelocity;
            this.canJump = canJump;
        }
        public JumpWallBlock() { }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //TODO: add checking for other Jump components the player would have
            entity.entityState = EntityState.ON_JUMP_WALL;
            entity.baseVelocity = new();
            if (!canJump)
            {
                new CollisionBlock().horizontalActions(entity, collision);
                return;
            }
            entity.onGround = true;
            if (!entity.TryGetComponent(out MoveVerticalComponent component)) return;
            if (entity.velocity.X > 0)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                if (component.JumpPressed)
                {
                    component.JumpingVertical(jumpStrength);
                    entity.velocity.X = -recoilVelocity;
                }
            }
            else if (entity.velocity.X < 0)
            {
                entity.Destinationrectangle.X = collision.Right;
                if (component.JumpPressed)
                {
                    component.JumpingVertical(jumpStrength);
                    entity.velocity.X = recoilVelocity;
                }
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);
        }
    }
}