using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class Portal : Block
    {
        public Portal otherPortal;
        public Portal() {}
        public Portal(Rectangle rectanglele) : base(rectanglele) { }
        public Portal(Rectangle rectanglele, Portal portalLink) : base(rectanglele)
        {
            otherPortal = portalLink;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
        }
    }
}