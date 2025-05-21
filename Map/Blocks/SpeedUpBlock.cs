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
        public override void horizontalActions(Entity entity, Rectangle collision, int _val)
        {
            if (entity.GetType() == typeof(Player) && _val == value)
            {
                //en parte escrito por copilot, gracias!!
                Player player = (Player)entity;
                entity.sprint += player.directionLeft ? -velocitySpeed : velocitySpeed;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision, int _val)
        {
            if (entity.GetType() == typeof(Player) && _val == value)
            {
                Player player = (Player)entity;
                entity.sprint += player.directionLeft ? -velocitySpeed : velocitySpeed;
            }
        }
    }
}