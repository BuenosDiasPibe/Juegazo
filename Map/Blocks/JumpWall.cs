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
        public override void horizontalActions(Entity entity, Rectangle collision, int _val)
        {
            if (entity.velocity.X > 0 && _val == value)
            {
                entity.Destrectangle.X = collision.Left - entity.Destrectangle.Width;
                entity.onGround = true;
                if(entity.jumpPressed) entity.pushBack = -20;
            }
            else if (entity.velocity.X < 0 && _val == value)
            {
                entity.Destrectangle.X = collision.Right;
                entity.onGround = true;
                if(entity.jumpPressed) entity.pushBack = 20;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision, int _val)
        {
            if (entity.velocity.Y > 0.0f && _val == value)
            {
                entity.Destrectangle.Y = collision.Top - entity.Destrectangle.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
            }
            else if (entity.velocity.Y < 0.0f && _val == value)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destrectangle.Y = collision.Bottom;
            }
        }
    }
}