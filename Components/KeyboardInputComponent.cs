using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.CustomTiledTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo.Components
{
    public class KeyboardInputComponent : Component
    {
        public bool keyLeft = false;
        public bool keyRight = false;
        public bool keyUp = false;
        public bool keyDown = false;
        public bool special1 = false;
        public bool special2 = false;

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
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();
            keyUp = keysUp.Any(key => currentState.IsKeyDown(key) && !prevState.IsKeyDown(key));
            keyDown = keysDown.Any(key => currentState.IsKeyDown(key));
            keyLeft = keysLeft.Any(key => currentState.IsKeyDown(key));
            keyRight = keysRight.Any(key => currentState.IsKeyDown(key));
            special1 = special1Keys.Any(key => currentState.IsKeyDown(key) && prevState.IsKeyDown(key));
            special2 = special2Keys.Any(key => currentState.IsKeyDown(key) && prevState.IsKeyDown(key));
            prevState = currentState;
        }
    }
}