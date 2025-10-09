using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class CheckPointBlock : Block
    {
        public Vector2 position = new();
        public int message = 0;
        public CheckPointBlock(Rectangle collisionRectangle) : base(collisionRectangle)
        {
            type = "CheckPointBlock";
            value = 17;
            position = new(collider.X, collider.Y);
        }
        public CheckPointBlock()
        {
            value = 17;
            type = "CheckPointBlock";
        }
        public CheckPointBlock(Rectangle collision, bool isEnabled, int message, Vector2 position) : base(collision)
        {
            value = 17;
            EnableCollisions = isEnabled;
            this.message = message;
            this.position = position;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (position == Vector2.Zero)
            {
                Console.WriteLine("how am i supposed to know this??'"); //shit i cant fix this unless i do a giant ass switch statement
                position = collider.Location.ToVector2();
            }
            if (entity.hasComponent<CanDieComponent>())
            {
                var canDieComponent = entity.getComponent<CanDieComponent>();
                canDieComponent.initialPosition = position;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (position == Vector2.Zero)
            {
                Console.WriteLine("how am i supposed to know this?? in vert");
                position = collider.Location.ToVector2();
            }
            horizontalActions(entity, collision);
        }
    }
}