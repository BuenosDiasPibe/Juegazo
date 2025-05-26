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
            velocitySpeed = 2;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.GetType() == typeof(Player))
            {
                //en parte escrito por copilot, gracias!!
                Player player = (Player)entity;
                player.sprint += player.directionLeft ? -velocitySpeed : velocitySpeed;
            }
        }

        public override void Update()
        {;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
        }
    }
}