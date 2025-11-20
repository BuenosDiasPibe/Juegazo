using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.EntityComponents
{
    public class CanDieComponent : Component
    {
        public Vector2 initialPosition;
        private float deathTimerSeconds = 0;
        public bool isDying { get; private set; } = false;
        private Color originalColor;
        // private SoundEffect thingie; //testing audio stuff

        public CanDieComponent(Vector2 initialPosition)
        {
            this.initialPosition = initialPosition;
        }
        public override void Start()
        {
            base.Start();
            // using (var stream = System.IO.File.OpenRead("Content/Sounds/song.wav"))
            // {
            //     thingie = SoundEffect.FromStream(stream);
            // }
        }

        public override void Update(GameTime gameTime)
        {
            if (Owner.health <= 0 && !isDying)
            {
                StartDeathSequence();
            }
            if (isDying)
            {
                Owner.entityState = EntityState.DYING;
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
                Camera.Instance.Position = pa;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Destroy()
        { }
    }
}