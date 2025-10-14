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
        public DoubleJumpComponent(int numJumps)
        {
            this.numJumps = numJumps;
            this.EnableDraw = true;
        }
        public DoubleJumpComponent(DoubleJump data)
        {
            numJumps = data.numberOfJumps;
            this.EnableDraw = true;
        }
        public void JumpingVertical(float jumpAmmount)
        {
            if ((Owner.onGround  || numJumps > timesJumped) && JumpPressed)
            {
                Owner.velocity.Y = -jumpAmmount;
                Owner.onGround = false;
                timesJumped++;
            }
        }
        public override void Destroy()
        {
            Owner.color = Color.White;
            Console.WriteLine("deleting doubleJump...");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Owner.texture, Owner.Destinationrectangle, Owner.sourceRectangle, Owner.color);
        }

        public override void Update(GameTime gameTime)
        {
            JumpPressed = Owner.hasComponent(typeof(KeyboardInputComponent)) && ((KeyboardInputComponent)Owner.GetComponent(typeof(KeyboardInputComponent))).keyUp;

            JumpingVertical(10);

            prevState = Keyboard.GetState();
            if (numJumps == timesJumped)
            {
                if (Owner.hasComponent(typeof(MoveVerticalComponent)) && Owner.GetComponent(typeof(MoveVerticalComponent)) is MoveVerticalComponent vir)
                {
                    vir.EnableUpdate = true;
                }
                Owner.RemoveComponent(this.GetType());
            }
        }
        private bool firstTime = false;
        public void Collisions(Entity entity)
        {
            if (Owner.collider.Intersects(entity.Destinationrectangle) && !firstTime && !entity.hasComponent(GetType()))
            {
                if (entity.hasComponent(typeof(MoveVerticalComponent)) && entity.GetComponent(typeof(MoveVerticalComponent)) is MoveVerticalComponent vir &&
                        vir.EnableUpdate)
                {
                    vir.EnableUpdate = false;
                }

                Type thisComponent = this.GetType();
                Owner.Visible = false;
                entity.AddComponent(thisComponent, Owner.RemoveComponent(thisComponent));
                Owner.color = Color.Red;
                Console.WriteLine("if this crashes im goin to kill myself");
                firstTime = true;
            }
        }
    }
}