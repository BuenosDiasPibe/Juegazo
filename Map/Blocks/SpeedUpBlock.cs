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
        private int velocitySpeed;
        public SpeedUpBlock(Rectangle collider)
            : base(collider)
        {
            value = 5;
            velocitySpeed = 3;
        }

        public SpeedUpBlock()
        {
            value = 5;
            velocitySpeed = 3;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            var moveComponent = entity.getComponent<MoveHorizontalComponent>();
            if (entity.velocity.X != 0)
            {
                entity.velocity.X += entity.directionLeft ? -velocitySpeed : velocitySpeed;
            }
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
        }
    }
}