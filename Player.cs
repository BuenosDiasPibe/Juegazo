using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public class Player : Sprite
    {
        public Vector2 velocity;
        public bool onGround {get;set;}
        private Color colorChange;
        private Color prevColor;
        public int sprint;
        public int numJumps;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = true;
            colorChange = Color.Aqua;
            prevColor = color;
            sprint = 0;
            numJumps = 0;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState, Viewport viewport)
        {
            velocity.X = 0;
            velocity.Y += 0.6f;
            velocity.Y = Math.Min(20, velocity.Y);

            if (keyboardState.IsKeyDown(Keys.A) | keyboardState.IsKeyDown(Keys.Left)) velocity.X = -10;
            if (keyboardState.IsKeyDown(Keys.D) | keyboardState.IsKeyDown(Keys.Right)) velocity.X = 10;
            if (onGround & keyboardState.IsKeyDown(Keys.Up) & !prevState.IsKeyDown(Keys.Up))
            {
                velocity.Y = -10;
                numJumps += 1;
            }
            if (onGround & keyboardState.IsKeyDown(Keys.W) & !prevState.IsKeyDown(Keys.W))
            {
                velocity.Y = -10;
                numJumps += 1;
            }
            if (keyboardState.IsKeyDown(Keys.S) | keyboardState.IsKeyDown(Keys.Down)) velocity.Y += 10;
            // quizas cambiar el dash para hacer que solo funcione con un boton y determinar la direccion a la que debe enviar al jugador, depende de como me levante mañana
            if (keyboardState.IsKeyDown(Keys.B) & !prevState.IsKeyDown(Keys.B)) sprint = -30;
            if (keyboardState.IsKeyDown(Keys.M) & !prevState.IsKeyDown(Keys.M)) sprint = 30;
            if (keyboardState.IsKeyDown(Keys.T) & !prevState.IsKeyDown(Keys.T)) velocity.Y = -10;

            if (sprint != 0) //idk how to make it look better, but it works!
            {
                velocity.X += sprint;
                if (sprint < 0)
                {
                    sprint++;
                }
                else if (sprint > 0)
                {
                    sprint--;
                }
            }
        }
    }
}