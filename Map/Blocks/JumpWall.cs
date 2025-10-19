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
    public class JumpWall : Block
    {
        private float jumpStrength = 11;
        public bool canJump = true;
        private float recoilVelocity = 10;
        public JumpWall(Rectangle collider) : base(collider)
        {
            type = "JumpWallBlock";
        }
        public JumpWall(Rectangle collider, int jumpSptrength, bool canJump, float recoilVelocity) : base(collider)
        {
            type = "JumpWallBlock";
            jumpStrength = jumpSptrength;
            this.recoilVelocity = recoilVelocity;
            this.canJump = canJump;
        }
        public JumpWall()
        {
            type = "JumpWallBlock";
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //TODO: add checking for other Jump components the player would have
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