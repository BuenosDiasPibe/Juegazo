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
        public int sprint;
        public int pushBack;
        public bool jumpPressed;
        public bool directionLeft;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = true;
            sprint = 0;
            pushBack = 0;
            directionLeft = true;
            jumpPressed = false;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState, Viewport viewport)
        {
            // Apply gravity
            velocity.Y += 0.6f;
            velocity.Y = Math.Min(20, velocity.Y);

            // Horizontal movement
            velocity.X = 0;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                velocity.X = -10;
                directionLeft = true;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                velocity.X = 10;
                directionLeft = false;
            }
            // Jumping
            jumpPressed = (keyboardState.IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) ||
                               (keyboardState.IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));
            if (onGround && jumpPressed)
            {
                velocity.Y = -10;
            }
            if (keyboardState.IsKeyDown(Keys.T) && !prevState.IsKeyDown(Keys.T))
            {
                velocity.Y = -10;
            }

            // Fast fall
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) velocity.Y += 10;

            // Dash (sprint)
            if (keyboardState.IsKeyDown(Keys.B) && !prevState.IsKeyDown(Keys.B))
                sprint = directionLeft ? -30 : 30;

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