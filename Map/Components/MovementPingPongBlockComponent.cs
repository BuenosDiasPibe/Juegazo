using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Components
{
    public class MovementPingPongBlockComponent : BlockComponent
    {
        public Vector2 newPosition = new();
        private Vector2 lastPosition = new();
        public Vector2 velocityToEntity = new();
        public float lerpAmmount = 0;
        private Rectangle initialPosition;
        private Rectangle endPosition;
        private float velocity = 0;
        public MovementPingPongBlockComponent(Rectangle initialPosition, Rectangle endPoisition, float velocity)
        {
            EnableUpdate = true;
            this.initialPosition = initialPosition;
            this.endPosition = endPoisition;
            this.velocity = velocity;
        }

        public override void Destroy()
        {
            Console.WriteLine("ayy");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 current = initialPosition.Location.ToVector2();
            Vector2 target = endPosition.Location.ToVector2();
            float distance = Vector2.Distance(current, target);
            float time = (float)(gameTime.TotalGameTime.TotalSeconds * velocity / distance);
            lerpAmmount = 2 * Math.Abs(time % 1 - 0.5f);

            newPosition = Vector2.Lerp(current, target, lerpAmmount);
            Owner.collider.Location = newPosition.ToPoint();

            velocityToEntity = Owner.collider.Location.ToVector2() - lastPosition;
            lastPosition = new(Owner.collider.X, Owner.collider.Y);
        }
    }
}