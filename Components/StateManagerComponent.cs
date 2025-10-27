using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    //im so sorry :((
    public class StateManagerComponent : Component
    {
        public override void Destroy()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(Owner.entityState != EntityState.TALKING)
            {
                if (Owner.velocity.Y > 0) Owner.entityState = EntityState.FALLING;
                if (Owner.onGround) Owner.entityState = EntityState.ON_GROUND;
                if (Owner.velocity.X != 0 && Owner.onGround) Owner.entityState = EntityState.WALKING;
                if (Owner.baseVelocity != Vector2.Zero) Owner.entityState = EntityState.UP_MOVING_BLOCK;
            }
        }
    }
}