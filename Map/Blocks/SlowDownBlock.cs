using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class SlowDownBlock : Block
    {
        public int slowingSpeed;
        public SlowDownBlock(Texture2D texture, Rectangle sourceRectangle, Rectangle destinationRectangle, Rectangle collisionRectangle, Color color)
            : base(texture, sourceRectangle, destinationRectangle, collisionRectangle, color)
        {
            value = 10;
            slowingSpeed = 1;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
                if (entity.velocity.X != 0)
                {
                    entity.velocity.X += entity.directionLeft ? slowingSpeed : -slowingSpeed;
                }
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}