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
            slowingSpeed = 1;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity is Player player && entity.velocity.X != 0)
            {
                if (player.velocity.X != 0)
                {
                player.velocity.X += player.directionLeft? slowingSpeed : -slowingSpeed;
                }
            }
        }


        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}