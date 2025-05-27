using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class SpeedUpBlock : BlockType
    {
        private int velocitySpeed;
        public SpeedUpBlock()
        {
            value = 5;
            velocitySpeed = 5;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity is Player player)
            {
                player.velocity.X += player.directionLeft ?  -velocitySpeed : velocitySpeed;
            }
        }

        public override void Update()
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
        }
    }
}