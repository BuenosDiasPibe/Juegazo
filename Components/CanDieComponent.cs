using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class CanDieComponent : Component
    {
        public Vector2 initialPosition;
        public CanDieComponent(Vector2 initialPosition)
        {
            this.initialPosition = initialPosition;
        }
        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            if (Owner.health <= 0)
            {
                Owner.velocity = new();
                Owner.Destinationrectangle.Location = initialPosition.ToPoint();
                Owner.health = 1;
                Owner.velocity = new();
                //doing this because i hate the smooth transition when i die, im trying to get rid of it, and it almost worked
                if (Owner.TryGetComponent<CameraToEntityComponent>(out var camComp))
                {
                    CameraToEntityComponent c = (CameraToEntityComponent)camComp;
                    var pa = new Vector2(initialPosition.X + c.lookAhead, initialPosition.Y);
                    c.cameraHorizontal = (int)pa.X;
                    c.cameraVertical = (int)pa.Y;
                    c.Camera.Position = pa;
                }
            }
        }
    }
}