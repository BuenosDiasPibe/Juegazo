using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class CameraToEntitySimpleComponent : Component
    {
        public Camera Camera { get; private set; }
        public CameraToEntitySimpleComponent(Camera camera) {
            Camera = camera;
        }

        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }
        public override void Update(GameTime gameTime)
        {
            Camera.Position = new Vector2(Owner.Destinationrectangle.X + Owner.Destinationrectangle.Width/2
                                          , Owner.Destinationrectangle.Y + Owner.Destinationrectangle.Height / 2
                                          );
        }
    }
}