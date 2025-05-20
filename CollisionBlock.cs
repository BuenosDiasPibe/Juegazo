using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Juegazo
{
    public class CollisionBlock : BlockType
    {
        public CollisionBlock()
        {
            value = 11;
        }
        public override void horizontalActions(Player player, Rectangle collision, int _val)
        {
            if (player.velocity.X > 0.0f && _val == value)
            {
                player.Destrectangle.X = collision.Left - player.Destrectangle.Width;
                player.onGround = true;
                if (player.jumpPressed) player.pushBack = -20;
            }
            else if (player.velocity.X < 0.0f && _val == value)
            {
                player.Destrectangle.X = collision.Right;
                player.onGround = true;
                if (player.jumpPressed) player.pushBack = 20;
            }
        }

        public override void verticalActions(Player player, Rectangle collision, int _val)
        {
            if (player.velocity.Y > 0.0f && _val == value)
            {
                player.Destrectangle.Y = collision.Top - player.Destrectangle.Height;
                player.velocity.Y = 1f;
                player.onGround = true;
            }
            else if (player.velocity.Y < 0.0f && _val == value)
            {
                player.velocity.Y *= 0.1f;
                player.Destrectangle.Y = collision.Bottom;
            }
        }
    }
}