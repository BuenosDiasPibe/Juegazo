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
        public bool JumpPressed { get; private set; } = false;
        public int jumpCounter = 0;
        private KeyboardState prevState = new KeyboardState();
        public MoveVerticalComponent()
        {
            jumpCounter = 0;
            JumpPressed = false;
        }
        public void JumpingVertical(float jumpAmmount) //make this a component and fuck me i guess
        {
            if (Owner.onGround && JumpPressed)
            {
                Owner.velocity.Y = -jumpAmmount;
                Owner.onGround = false;
            }
        }

        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {//this method is fucking stupid

            JumpPressed = (Keyboard.GetState().IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) || (Keyboard.GetState().IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));

            //jumpCheat
            if (Keyboard.GetState().IsKeyDown(Keys.T) && !prevState.IsKeyDown(Keys.T)) Owner.velocity.Y = -20;

            //Fast fall
            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) Owner.velocity.Y = 10;
            JumpingVertical(10);
            prevState = Keyboard.GetState();
        }
    }
}