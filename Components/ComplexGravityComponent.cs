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
        private float currentFalling = 0;
        public CardinalPoint cardinalPoint = CardinalPoint.North;

        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            currentFalling += FALLING_VELOCITY;
            switch (cardinalPoint)
            {
                case CardinalPoint.North:
                    Owner.velocity.Y = Math.Min(MAX_FALLING, currentFalling);
                    break;
                case CardinalPoint.South:
                    Owner.velocity.Y = -Math.Min(MAX_FALLING, currentFalling);
                    break;
                case CardinalPoint.East:
                    Owner.velocity.X = -Math.Min(MAX_FALLING, currentFalling);
                    break;
                case CardinalPoint.West:
                    Owner.velocity.X = Math.Min(MAX_FALLING, currentFalling);
                    break;
            }
        }
    }
}