using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class ComplexGravityComponent : Component
    {
        private const float MAX_FALLING = 10;
        private const float FALLING_VELOCITY = 0.6f;
        public CardinalPoint cardinalPoint = CardinalPoint.North;

        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            switch (cardinalPoint)
            {
                case CardinalPoint.North:
                    if (Owner.velocity.Y > MAX_FALLING) break;
                    Owner.velocity.Y += FALLING_VELOCITY;
                    break;
                case CardinalPoint.South:
                    break;
                case CardinalPoint.East:
                    break;
                case CardinalPoint.West:
                    break;
            }
            //Owner.velocity.Y = Math.Min(MAX_FALLING, Owner.velocity.Y);
        }
    }
}