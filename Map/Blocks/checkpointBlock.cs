using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class CheckpointBlock : BlockType
    {
        public CheckpointBlock()
        {
            value = 17;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity is Player player)
            {
                player.initialPosition = new Vector2(collision.X, collision.Y);
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}