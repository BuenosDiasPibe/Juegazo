using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class CheckpointBlock : Block
    {
        public CheckpointBlock(Rectangle collisionRectangle) : base(collisionRectangle)
        {
            value = 17;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent<CanDieComponent>())
            {
                var canDieComponent = entity.getComponent<CanDieComponent>();
                canDieComponent.initialPosition = new Vector2(collision.X, collision.Y);
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