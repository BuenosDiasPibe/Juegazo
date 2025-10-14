using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class OneWayBlock : Block
    {
        //TODO: add enum with orientation
        public OneWayBlock(Rectangle collider)
            : base(collider)
        {
            type = "OneWayBlock";
        }
        public OneWayBlock()
        {
            type = "OneWayBlock";
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            bool collidesWithTop = entity.Destinationrectangle.Bottom > collision.Top && entity.Destinationrectangle.Top < collision.Top;
            if (entity.velocity.Y > 0.0f && collidesWithTop)
            {
                entity.baseVelocity = new();
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
                if (entity.hasComponent(typeof(MoveVerticalComponent)))
                {
                    ((MoveVerticalComponent)entity.GetComponent(typeof(MoveVerticalComponent))).jumpCounter = 0;
                }
            }
        }
    }
}