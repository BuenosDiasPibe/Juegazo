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
        private const float MOVEMENT_SPEED = 5f;
        private const float MAX_SPEED = 100f;
        private float dashCounter;
        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            bool movingLeft = Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
            bool movingRight = Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            // Horizontal movement
            if (movingLeft)
            {
                if (!(Owner.velocity.X <= -5))
                {
                    Owner.velocity.X += -MOVEMENT_SPEED; //TODO: velocity is not always checked, you can exceed the max velocity if you press the same key multiple times (fuck)
                }
                Owner.directionLeft = true;
            }
            if (movingRight)
            {
                if (!(Owner.velocity.X >= 5))
                {
                    Owner.velocity.X += MOVEMENT_SPEED;
                }
                Owner.directionLeft = false;
            }
            if (movingLeft && movingRight)
            {
                Owner.velocity.X = 0;
            }
            // Dash (sprint)
            if (dashCounter > 0 && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                int basedOnDirection = Owner.directionLeft ? -7 : 7;
                Owner.velocity.X += basedOnDirection;
                dashCounter--;
            }

            if (!movingLeft && !movingRight)
            {
                if (Math.Abs(Owner.velocity.X) > 1)
                {
                    Owner.velocity.X += Owner.velocity.X > 0 ? -1 : 1;
                }
                else
                {
                    Owner.velocity.X = 0;
                }
            }
            //max speed limit
            Owner.velocity.X = Math.Min(Math.Max(Owner.velocity.X, -MAX_SPEED), MAX_SPEED);

        }
    }
}