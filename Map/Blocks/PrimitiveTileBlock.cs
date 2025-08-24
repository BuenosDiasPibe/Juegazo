using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class PrimitiveTileBlock : Block
    {
        public PrimitiveTileBlock(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Rectangle collider, Color color) : base(texture, sourceRectangle, Destrectangle, collider, color)
        {
            EnableCollisions = false;
        }

        public override void horizontalActions(Entity entity, Rectangle collision)
        {
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            throw new NotImplementedException();
        }
    }
}