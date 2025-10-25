using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo.Components
{
    public class MoveVerticalComponent : Component
    {
        public bool JumpPressed = false;
        private bool fastFall = false;
        private bool jumpCheat = false;
        public MoveVerticalComponent()
        {
            JumpPressed = false;
        }
        public void JumpingVertical(float jumpAmmount)
        {
            if (Owner.onGround && JumpPressed)
            {
                Owner.velocity.Y = -jumpAmmount;
                Owner.onGround = false;
            }
        }
        public override void Destroy() {}
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            if(Owner.TryGetComponent(out KeyboardInputComponent c))
            {
                JumpPressed = c.btnUp;
                //jumpCheat = c.btnpSpecial1;
                // fastFall = c.btnSpecial2;
            }
            if (jumpCheat) Owner.velocity.Y = -20;
            if(Owner.velocity.Y < 0)
            {
                Owner.entityState = EntityState.JUMPING;
            }
            // if (fastFall) Owner.velocity.Y += 2;
            JumpingVertical(10);
        }
    }
}