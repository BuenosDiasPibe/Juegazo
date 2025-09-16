using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class CameraToEntityComponent : Component
    {
        public Camera Camera { get; private set; }
        public int cameraHorizontal = 0;
        public int cameraVertical = 0;
        public int lookAhead = 0;
        public CameraToEntityComponent(Camera camera)
        {
            Camera = camera;
        }

        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            if (Owner.directionLeft) lookAhead = -200;
            else lookAhead = 200;
            
            cameraHorizontal = (int)MathHelper.Lerp(cameraHorizontal, Owner.Destinationrectangle.X + lookAhead + Owner.Destinationrectangle.Width / 2, 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);

            cameraVertical = Owner.Destinationrectangle.Y + Owner.Destinationrectangle.Height / 2;

            Vector2 targetPosition = new Vector2(cameraHorizontal, cameraVertical);
            Camera.Position = Vector2.Lerp(Camera.Position, targetPosition, 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);
        }
    }
}