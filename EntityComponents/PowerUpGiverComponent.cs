using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Components
{
    public class PowerUpGiverComponent : Component
    {
        public Component componentGived { get; set; }
        public bool touchedBefore = false;
        public PowerUpGiverComponent(Component c)
        {
            componentGived = c;
            this.EnableDraw = false;
        }
        public PowerUpGiverComponent()
        {
            this.EnableDraw = false;
        }
        public override void Destroy()
        {
            Console.WriteLine("deleting PowerUpGiver");
        }
        public override void Start()
        {
            if(componentGived == null)
            {
                throw new Exception("no component has been added");
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        {
            if (touchedBefore)
            {
                Owner.color = Color.Gray;
                _ = DelayedTouch();
            }
        }

        private async Task DelayedTouch()
        {
            await Task.Delay(5000);
            touchedBefore = false;
            Owner.color = Color.White;
        }
        public void Collision(Entity entity)
        {
            if (entity.Destinationrectangle.Intersects(Owner.collider) && !touchedBefore && !entity.hasComponent(componentGived.GetType()))
            {
                entity.AddComponent(componentGived.GetType(), componentGived);
                touchedBefore = true;
                entity.entityState = EntityState.POWERUP;
            }
        }
    }
}