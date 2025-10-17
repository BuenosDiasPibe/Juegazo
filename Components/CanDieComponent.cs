using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class CanDieComponent : Component
    {
        public Vector2 initialPosition;
        private float deathTimerSeconds = 0;
        private bool isDying = false;
        private Color originalColor;
        
        public CanDieComponent(Vector2 initialPosition)
        {
            this.initialPosition = initialPosition;
        }

        public override void Update(GameTime gameTime)
        {
            if (Owner.health <= 0 && !isDying)
            {
                StartDeathSequence();
            }
            if (isDying)
            {
                deathTimerSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                Owner.color = Color.Red * ((float)(1 + Math.Sin(deathTimerSeconds * 10))/2);

                if (deathTimerSeconds >= 1f)
                {
                    RespawnPlayer();
                }
            }
        }

        private void StartDeathSequence()
        {
            isDying = true;
            deathTimerSeconds = 0;
            originalColor = Owner.color;

            // Disable player input
            if (Owner.TryGetComponent<KeyboardInputComponent>(out var input))
            {
                input.EnableUpdate = false;
                input.disablePlayerInput();
            }
        }

        private void RespawnPlayer()
        {
            isDying = false;
            Owner.color = originalColor;
            Owner.Destinationrectangle.Location = initialPosition.ToPoint();
            Owner.health = 1;
            Owner.velocity = new();

            // Re-enable player input
            if (Owner.TryGetComponent<KeyboardInputComponent>(out var input))
            {
                input.EnableUpdate = true;
            }

            // Reset camera position
            if (Owner.TryGetComponent<CameraToEntityComponent>(out var c))
            {
                var pa = new Vector2(initialPosition.X + c.lookAhead, initialPosition.Y);
                c.cameraHorizontal = (int)pa.X;
                c.cameraVertical = (int)pa.Y;
                c.Camera.Position = pa;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Destroy()
        { }
    }
}