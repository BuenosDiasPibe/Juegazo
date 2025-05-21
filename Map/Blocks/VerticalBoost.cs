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
        public override void horizontalActions(Entity entity, Rectangle collision, int _val)
        {
            verticalActions(entity, collision, _val);
        }

        public override void verticalActions(Entity entity, Rectangle collision, int _val)
        {
            if (_val == value)
            {
                entity.onGround = true;
                entity.verticalBoost += vertBoost;
            }
            else if (entity.velocity.Y < 0.0f && _val == value)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destrectangle.Y = collision.Bottom;
            }
        }
    }
}