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
        public bool dash = false;
        private float colorTransitioned = 0;
        private Color colorOnDash = Color.Coral;
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
                dash = c.btnpSpecial2;
            }

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
            if (dash)
            {
                float basedOnDirection = Owner.directionLeft ? -30 : 30;
                //colorTransitioned = 1;
                Owner.velocity.X = basedOnDirection;
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
            Owner.velocity.X = Math.Min(Math.Max(Owner.velocity.X, -MAX_SPEED), MAX_SPEED);

            //this should go on a separate event handler or something, but its funny for me rn
            //Owner.color = Color.Lerp(Color.White, colorOnDash, colorTransitioned);
            //colorTransitioned = Math.Max(0, colorTransitioned - 0.05f);
        }
    }
}