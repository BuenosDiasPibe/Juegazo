using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class JumpWall : Block
    {
        private float jumpStrength;
        public JumpWall(Rectangle collider)
            : base(collider)
        {
            value = 1;
            jumpStrength = 0;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //change this to use the new ECS model
            if (entity is Player player)
            {
                player.onGround = true;
                player.jumpCounter = 0;
                player.hasJumpedWall = true;
                if (player.velocity.X > 0)
                {
                    entity.collider.X = collision.Left - entity.collider.Width;
                    if (player.jumpPressed)
                    {
                        jumpMechanic(player);
                        player.velocity.X = -11;
                    }
                }
                else if (player.velocity.X < 0)
                {
                    entity.collider.X = collision.Right;
                    if (player.jumpPressed)
                    {
                        jumpMechanic(player);
                        player.velocity.X = 11;
                    }
                }
                else //yo cuando code repetition (es importante te lo juro)
                {
                    if (entity.velocity.X > 0)
                    {
                        entity.collider.X = collision.Left - entity.collider.Width;
                    }
                    else if (entity.velocity.X < 0)
                    {
                        entity.collider.X = collision.Right;
                    }
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
            if(entity is Player player)
                player.hasJumpedWall = false;
            if (entity.velocity.Y > 0.0f)
            {
                entity.collider.Y = collision.Top - entity.collider.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.collider.Y = collision.Bottom;
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}