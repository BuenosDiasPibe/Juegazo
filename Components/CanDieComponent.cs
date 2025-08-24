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
            Enable = false;
        }
        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        { }
    }
}