using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class VerticalBoost : Block
    {
        public float vertBoost;
        public VerticalBoost(Texture2D texture, Rectangle sourceRectangle, Rectangle destinationRectangle, Rectangle collisionRectangle, Color color)
            : base(texture, sourceRectangle, destinationRectangle, collisionRectangle, color)
        {
            value = 8;
            vertBoost = -1;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            verticalActions(entity, collision);
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.Y > 0)
            {
                entity.velocity.Y = -20; //dont know why i didnt made this months ago....
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}