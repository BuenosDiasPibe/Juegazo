using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class SpeedUpBlock : Block
    {
        private int velocitySpeed = 3;
        public SpeedUpBlock(Rectangle collider) : base(collider) { }

        public SpeedUpBlock() { }
        public SpeedUpBlock(Rectangle collider, int velocitySpeed) : base(collider)
        {
            this.velocitySpeed = velocitySpeed;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            var moveComponent = entity.TryGetComponent(out MoveHorizontalComponent c);
            if (entity.velocity.X != 0)
            {
                entity.velocity.X += entity.directionLeft ? -velocitySpeed : velocitySpeed;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
        }
    }
}