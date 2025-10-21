using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo.Components
{
    public class MoveHorizontalComponent : Component
    {
        private const float MOVEMENT_SPEED = 8f;
        private const float MAX_SPEED = 100f;
        private float dashCounter;
        public bool movingLeft = false;
        public bool movingRight = false;
        public bool movingUp = false;
        public bool movingDown = false;
        public bool dash = false;
        public bool dashed = false;
        public const float dashV = 15f;
        public const float dashH = 15f;
        public const int FLOAT_DASH_LIMIT = 20;
        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(Owner.TryGetComponent(out KeyboardInputComponent c))
            {
                movingLeft = c.btnLeft;
                movingRight = c.btnRight;
                movingUp = c.btnUp;
                movingDown = c.btnDown;
                dash = c.btnpSpecial2;
            }

            // Horizontal movement
            if (movingLeft)
            {
                if (!(Owner.velocity.X <= -MOVEMENT_SPEED))
                {
                    float acceleration = MOVEMENT_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
                    Owner.velocity.X -= acceleration;
                }
                Owner.directionLeft = true;
            }
            if (movingRight)
            {
                if (!(Owner.velocity.X >= MOVEMENT_SPEED))
                {
                    float acceleration = MOVEMENT_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
                    Owner.velocity.X += acceleration;
                }
                Owner.directionLeft = false;
            }
            if (movingLeft && movingRight)
            {
                Owner.velocity.X = 0;
            }
            // Dash
            if (dash && !dashed)
            {
                Owner.entityState = EntityState.DASHING;
                if (!(movingUp || movingDown || movingRight || movingLeft))
                {
                    Owner.velocity.X += Owner.directionLeft ? -dashH : dashH;
                }
                else
                {
                    if (movingUp || movingDown && !(movingUp && movingDown))
                    {
                        Owner.velocity.Y = movingUp ? -dashV : dashV;
                    }
                    if (movingLeft || movingRight)
                    {
                        Owner.velocity.X += movingLeft ? -dashH : dashH;
                    }
                }
                dashed = true;
            }


            if (Math.Abs(Owner.velocity.X) > MOVEMENT_SPEED)
            {
                Owner.velocity.X += Owner.velocity.X > 0 ?
                    -0.05f : 0.05f;
            }
            if (!movingLeft && !movingRight)
            {
                if (Math.Abs(Owner.velocity.X) > 1)
                {
                    Owner.velocity.X += Owner.velocity.X > 0 ? 
                        -1 : 1;
                }
                else
                {
                    Owner.velocity.X = 0;
                }
            }

            // Reset dash when speed is low enough
            if (Math.Abs(Owner.velocity.X) < FLOAT_DASH_LIMIT && Owner.onGround)
            {
                dashed = false;
            }
            Owner.velocity.X = Math.Min(Math.Max(Owner.velocity.X, -MAX_SPEED), MAX_SPEED);
        }
    }
}