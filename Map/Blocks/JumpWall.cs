using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class JumpWall : BlockType
    {
        public JumpWall()
        {
            value = 1;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            //change to only allow for enemies or player to use this block, not Guns nor Collectables will ever use them
            if (entity.GetType() == typeof(Player))
            {
                Player player = (Player)entity;
                if (player.velocity.X > 0)
                {
                    entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
                    player.onGround = true;
                    if (player.jumpPressed) player.pushBack = -20;
                }
                else if (player.velocity.X < 0)
                {
                    entity.Destinationrectangle.X = collision.Right;
                    player.onGround = true;
                    if (player.jumpPressed) player.pushBack = 20;
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

        public override void Update()
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
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