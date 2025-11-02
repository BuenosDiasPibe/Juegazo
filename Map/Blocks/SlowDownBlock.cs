using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class SlowDownBlock : Block
    {
        public int slowingSpeed = 1;
        public SlowDownBlock(Rectangle collider) : base(collider) { }
        public SlowDownBlock(Rectangle collider, int slowingSpeed) : base(collider)
        {
            this.slowingSpeed = slowingSpeed;
        }
        public SlowDownBlock() { }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.X != 0)
            {
                entity.baseVelocity = new();
                entity.velocity.X += entity.directionLeft ? slowingSpeed : -slowingSpeed;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}