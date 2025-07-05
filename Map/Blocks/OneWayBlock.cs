using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class OneWayBlock : BlockType
    {
        public OneWayBlock()
        {
            value = 12;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        { }


        public override void verticalActions(Entity entity, Rectangle collision)
        {
            bool collidesWithTop = entity.Destinationrectangle.Bottom > collision.Top && entity.Destinationrectangle.Top < collision.Top;
            if (entity.velocity.Y > 0.0f && collidesWithTop)
            {
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
                if (entity is Player player)
                {
                    player.jumpCounter = 0;
                }
            }
        }
    }
}