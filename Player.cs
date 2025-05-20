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
        public bool onGround { get; set; }
        private Color colorChange;
        private Color prevColor;
        public int sprint;
        public int numJumps { get; set; }
        public int pushBack;
        public bool jumpPressed;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = true;
            colorChange = Color.Aqua;
            prevColor = color;
            sprint = 0;
            numJumps = 0;
            pushBack = 0;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState, Viewport viewport)
        {
            // Apply gravity
            velocity.Y += 0.6f;
            velocity.Y = Math.Min(30, velocity.Y);

            // Horizontal movement
            velocity.X = 0;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                velocity.X = -10;
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                velocity.X = 10;

            // Jumping
            jumpPressed = (keyboardState.IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) ||
                               (keyboardState.IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));
            if (onGround && jumpPressed)
            {
                velocity.Y = -15;
                numJumps++;
            }
            if (keyboardState.IsKeyDown(Keys.T) && !prevState.IsKeyDown(Keys.T))
            {
                velocity.Y = -15;
            }

            // Fast fall
                if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) velocity.Y += 10;

            // Dash (sprint)
            if (keyboardState.IsKeyDown(Keys.B) && !prevState.IsKeyDown(Keys.B))
                sprint = -30;
            if (keyboardState.IsKeyDown(Keys.M) && !prevState.IsKeyDown(Keys.M))
                sprint = 30;

            // Apply sprint
            if (sprint != 0)
            {
                velocity.X += sprint;
                sprint += sprint > 0 ? -1 : 1;
            }

            // Apply 'pushBack'
            if (pushBack != 0)
            {
                velocity.X += pushBack;
                pushBack += pushBack > 0 ? -1 : 1;
            }
        }
    }
}