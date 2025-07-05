using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class KillBlock : BlockType
    {
        public KillBlock()
        {
            value = 18;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.health--;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.health--;
        }
    }
}