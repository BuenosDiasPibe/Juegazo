using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Map.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class EntitiesInteractionsComponent : Component
    {
        public List<Entity> entities;
        public EntitiesInteractionsComponent(List<Entity> entities)
        {
            this.entities = entities;
        }
        public override void Destroy()
        {
            Console.WriteLine("oh fuck");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
        //TODO: if you collide with more than one entity with NPCComponent, there should only be one instance
        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                if (entity.TryGetComponent(out NPCComponent n))
                {
                    n.Collisions(Owner);
                }
                if (entity.TryGetComponent(out PowerUpGiverComponent powerUp))
                {
                    powerUp.Collision(Owner);
                }
            }
        }
    }
}