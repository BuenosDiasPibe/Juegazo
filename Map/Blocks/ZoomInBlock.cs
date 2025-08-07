using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class ZoomInBlock : BlockType
    {
        public bool canChange =false;
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity is Player player)
            {
                if (canChange)
                {
                    player.camera.Zoom = MathHelper.Lerp(player.camera.Zoom, player.camera.Zoom + 1, 0.1f);
                }
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity is Player player)
            {
                if (canChange)
                {
                    player.camera.Zoom = MathHelper.Lerp(player.camera.Zoom, player.camera.Zoom + 1, 0.1f);
                }
            }
        }
    }
}