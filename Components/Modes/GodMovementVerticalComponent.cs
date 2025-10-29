using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo.Components
{
    public class GodMovementVerticalModeComponent : Component
    {
        private readonly float VERTICALMOVEMENT = 5f;
        public override void Destroy()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                Owner.velocity.Y = -VERTICALMOVEMENT;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                Owner.velocity.Y = VERTICALMOVEMENT;
            else
                Owner.velocity.Y = 0;
        }
    }
}