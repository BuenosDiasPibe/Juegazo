using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class ComplexGravityComponent : Component
    {
        private const float MAX_FALLING = 10;
        private const float FALLING_VELOCITY = 0.6f;

        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            if (Owner.velocity.Y > MAX_FALLING) return;
            Owner.velocity.Y += FALLING_VELOCITY;
        }
    }
}