using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class ZoomInBlock : Block
    {
        public ZoomInBlock(Rectangle collider)
            : base(collider)
        { }
        public ZoomInBlock() {}
        public bool canChange = false;
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent<CameraToEntityComponent>())
            {
                var cameraComponent = entity.getComponent<CameraToEntityComponent>();
                cameraComponent.Camera.Zoom = MathHelper.Lerp(cameraComponent.Camera.Zoom, cameraComponent.Camera.Zoom + 1, 0.1f); ;
            }
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent<CameraToEntityComponent>())
            {
                var cameraComponent = entity.getComponent<CameraToEntityComponent>();
                cameraComponent.Camera.Zoom = MathHelper.Lerp(cameraComponent.Camera.Zoom, cameraComponent.Camera.Zoom + 1, 0.1f); ;
            }
        }
    }
}