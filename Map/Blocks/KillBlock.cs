using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class KillBlock : Block
    {
        public KillBlock(Texture2D texture, Rectangle sourceRectangle, Rectangle destinationRectangle, Rectangle collisionRectangle, Color color)
            : base(texture, sourceRectangle, destinationRectangle, collisionRectangle, color)
        {
            value = 18;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.health--;
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.health--;
        }
    }
}