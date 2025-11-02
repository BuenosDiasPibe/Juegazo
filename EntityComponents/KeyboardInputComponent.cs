using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.CustomTiledTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo.EntityComponents
{
    public class KeyboardInputComponent : Component
    {
        public bool btnLeft, btnRight, btnUp, btnDown, btnSpecial1, btnSpecial2;
        public bool btnpLeft, btnpRight, btnpUp, btnpDown, btnpSpecial1, btnpSpecial2;

        public List<Keys> keysUp = new() { Keys.Up, Keys.W, Keys.Space };
        public List<Keys> keysDown = new() { Keys.S, Keys.Down };
        public List<Keys> keysLeft = new() { Keys.A, Keys.Left };
        public List<Keys> keysRight = new() { Keys.D, Keys.Right };
        public List<Keys> special1Keys = new() { Keys.X, Keys.J };
        public List<Keys> special2Keys = new() { Keys.C, Keys.K };

        public KeyboardInputComponent()
        {
            this.EnableUpdate = true;
            this.EnableDraw = false;
        }
        private KeyboardState prevState = new();
        public override void Destroy()
        {
            Console.WriteLine("destroying keyboard component");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();
            btnUp = keysUp.Any(key => currentState.IsKeyDown(key));
            btnDown = keysDown.Any(key => currentState.IsKeyDown(key));
            btnLeft = keysLeft.Any(key => currentState.IsKeyDown(key));
            btnRight = keysRight.Any(key => currentState.IsKeyDown(key));
            btnSpecial1 = special1Keys.Any(key => currentState.IsKeyDown(key));
            btnSpecial2 = special2Keys.Any(key => currentState.IsKeyDown(key));

            btnpUp = keysUp.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));
            btnpDown = keysDown.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));
            btnpLeft = keysLeft.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));
            btnpRight = keysRight.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));
            btnpSpecial1 = special1Keys.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));
            btnpSpecial2 = special2Keys.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));

            prevState = currentState;
        }
        public void disablePlayerInput()
        {
            btnLeft = btnRight = btnUp = btnDown = btnSpecial1 = btnSpecial2 = false;
            btnpLeft = btnpRight = btnpUp = btnpDown = btnpSpecial1 = btnpSpecial2 = false;
        }
    }
}