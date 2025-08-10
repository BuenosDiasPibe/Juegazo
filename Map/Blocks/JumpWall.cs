using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class JumpWall : BlockType
    {
        private float jumpStrength;
        public JumpWall()
        {
            value = 1;
            jumpStrength = 0;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //change to only allow for enemies or player to use this block, not Guns nor Collectables will ever use them
            //TODO: interpolar CollisionBlock a el checking de este bloque
            if (entity is Player player)
            {
                player.onGround = true;
                player.jumpCounter = 0;
                player.hasJumpedWall = true;
                if (player.velocity.X > 0)
                {
                    entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                    if (player.jumpPressed)
                    {
                        player.incrementJumps++;
                        jumpMechanic(player);
                        player.velocity.X = -11;
                    }
                }
                else if (player.velocity.X < 0)
                {
                    entity.Destinationrectangle.X = collision.Right;
                    if (player.jumpPressed)
                    {
                        player.incrementJumps++;
                        jumpMechanic(player);
                        player.velocity.X = 11;
                    }
                }
                else //yo cuando code repetition (es importante te lo juro)
                {
                    if (entity.velocity.X > 0)
                    {
                        entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                    }
                    else if (entity.velocity.X < 0)
                    {
                        entity.Destinationrectangle.X = collision.Right;
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
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}