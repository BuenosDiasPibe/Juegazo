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
    public class DoubleJumpComponent : Component
    {
        public bool JumpPressed { get; private set; } = false;
        public int timesJumped = 0;
        public int numJumps = 5;
        private KeyboardState prevState = new KeyboardState();
        private Color prevOwnerColor;
        public DoubleJumpComponent(int numJumps)
        {
            this.numJumps = numJumps;
            this.EnableDraw = false;
        }
        public DoubleJumpComponent(DoubleJump data)
        {
            numJumps = data.numberOfJumps;
            this.EnableDraw = false;
        }
        public void JumpingVertical(float jumpAmmount)
        {
            if (JumpPressed)
            {
                if(Owner.onGround)
                {
                    Owner.entityState = EntityState.JUMPING;
                }
                else
                {
                    Owner.entityState = EntityState.JUMPING_ON_AIR;
                    timesJumped += 1;
                }
                Owner.velocity.Y = -jumpAmmount;
                Owner.onGround = false;
            }
        }
        public override void Start()
        {
            if (Owner.TryGetComponent(out MoveVerticalComponent mv))
            {
                mv.EnableUpdate = false;
            }
            prevOwnerColor = Owner.color;
            Owner.color = Color.Gold;
        }
        public override void Destroy()
        {
            timesJumped = 0;
            Owner.color = prevOwnerColor;
            Console.WriteLine("deleting doubleJump...");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            if(Owner.TryGetComponent(out KeyboardInputComponent c))
            {
                JumpPressed = c.btnpUp;
            }

            JumpingVertical(10);

            prevState = Keyboard.GetState();
            if (timesJumped >= numJumps)
            {
                if(Owner.TryGetComponent(out MoveVerticalComponent vir))
                {
                    vir.EnableUpdate = true;
                }
                Owner.RemoveComponent(this.GetType());
            }
        }
    }
}