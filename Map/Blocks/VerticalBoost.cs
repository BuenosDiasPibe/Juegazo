using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class VerticalBoost : BlockType
    {
        public float vertBoost;
        public VerticalBoost()
        {
            value = 8;
            vertBoost = -1;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            verticalActions(entity, collision);
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.GetType() == typeof(Player))
            {
                Player player = (Player)entity;
                entity.onGround = true;
                player.verticalBoost += vertBoost;
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}