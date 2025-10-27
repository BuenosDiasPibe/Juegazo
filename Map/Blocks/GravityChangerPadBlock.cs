using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class GravityChangerPadBlock : Block
    {
        //idk if it should change to how pads work in gd... maybe
        public FACES whichDirection;
        public GravityChangerPadBlock() { }
        public GravityChangerPadBlock(Rectangle collider, FACES whichDirection) : base(collider)
        {
            this.whichDirection = whichDirection;
            this.colorBlock = whichDirection switch
            {
                FACES.TOP => new Color(243, 166, 247),
                FACES.BOTTOM => new Color(137, 180, 250),
                FACES.LEFT => new Color(249, 226, 175),
                FACES.RIGHT => new Color(166, 227, 161),
                _ => throw new NotImplementedException(),
            };
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.direction != whichDirection)
                entity.direction = whichDirection;

        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.direction != whichDirection)
                entity.direction = whichDirection;
        }
    }
}
        