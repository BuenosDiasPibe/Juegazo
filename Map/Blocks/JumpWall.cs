using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class JumpWall : Block
    {
        private float jumpStrength = 7;
        public bool canJump;
        private float recoilVelocity = 20;
        public JumpWall(Rectangle collider)
            : base(collider)
        {
            value = 1;
        }
        public JumpWall(Rectangle collider, int jumpSptrength, bool canJump, float recoilVelocity) : base(collider)
        {
            this.jumpStrength = jumpSptrength;
            this.recoilVelocity = recoilVelocity;
            this.canJump = canJump;
        }
        public JumpWall()
        {
            value = 1;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (!canJump)
            {
                new CollisionBlock().horizontalActions(entity, collision);
                return;
            }
            //change this to use the new ECS model
            entity.onGround = true;
            MoveVerticalComponent verticalMovement = (MoveVerticalComponent)entity.getComponent(typeof(MoveVerticalComponent));
            if (entity.velocity.X > 0)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                if (verticalMovement.JumpPressed)
                {
                    verticalMovement.JumpingVertical(jumpStrength);
                    entity.velocity.X = -recoilVelocity;
                }
            }
            else if (entity.velocity.X < 0)
            {
                entity.Destinationrectangle.X = collision.Right;
                if (verticalMovement.JumpPressed)
                {
                    verticalMovement.JumpingVertical(jumpStrength);
                    entity.velocity.X = recoilVelocity;
                    Console.WriteLine($"added {11} to entity velocity, now velocity is {entity.velocity.X}");
                }
            }
        }
        private void jumpMechanic(Player player)
        {
            jumpStrength = 11 + player.velocity.Y;
            player.Jumping(jumpStrength);
        }


        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}