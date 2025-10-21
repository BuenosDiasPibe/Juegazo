using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class WaterComponent : Component
    {
        public int velocity = 8;
        public WaterComponent()
        {
            this.EnableUpdate = true;
            this.EnableDraw = false;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Destroy()
        {
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Owner.touchingWaterBlock)
            {
                Owner.onGround = false;
                if (Owner.TryGetComponent(out MoveHorizontalComponent hz))
                {
                    hz.EnableUpdate = false;
                }
                if (Owner.TryGetComponent(out MoveVerticalComponent vc))
                {
                    vc.EnableUpdate = false;
                }
                if(Owner.TryGetComponent(out ComplexGravityComponent cgc))
                {
                    cgc.EnableUpdate = false;
                }
                if (Owner.TryGetComponent(out KeyboardInputComponent kic))
                { // this is so pico8-pilled
                    // Apply friction to slow down movement gradually
                    float friction = 0.85f;
                    Owner.velocity *= friction;

                    // Apply input
                    if (kic.btnUp) Owner.velocity.Y = -velocity;
                    if (kic.btnDown) Owner.velocity.Y = velocity;
                    if (kic.btnLeft) Owner.velocity.X = -velocity;
                    if (kic.btnRight) Owner.velocity.X = velocity;
                }
            }
            else
            {
                this.EnableUpdate = false;
                if (Owner.TryGetComponent(out MoveHorizontalComponent hz))
                {
                    hz.EnableUpdate = true;
                }
                if (Owner.TryGetComponent(out MoveVerticalComponent vc))
                {
                    vc.EnableUpdate = true;
                }
                if(Owner.TryGetComponent(out ComplexGravityComponent cgc))
                {
                    cgc.EnableUpdate = true;
                }
            }
        }
    }
}