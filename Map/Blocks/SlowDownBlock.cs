using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class SlowDownBlock : BlockType
    {
        public int slowingSpeed;
        public SlowDownBlock()
        {
            value = 10;
            slowingSpeed = 2;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.GetType() == typeof(Player) && entity.velocity.X != 0)
            {
                Player player = (Player)entity;
                player.sprint += player.directionLeft ? slowingSpeed : -slowingSpeed;
                //si la velocidad de sprint es mayor que el opuesto de la velocidad (-velocidad es el opuesto)
                if (Math.Abs(player.sprint) > Math.Abs(-entity.velocity.X)) player.sprint = -entity.velocity.X * 0.5f;
            }
        }

        public override void Update()
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}