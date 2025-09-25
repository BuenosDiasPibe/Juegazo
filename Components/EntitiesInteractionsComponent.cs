using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class EntitiesInteractionsComponent : Component
    {
        List<Entity> entities;
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

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                if (!entity.hasComponent(typeof(NPCComponent))) continue;
                if (entity.getComponent(typeof(NPCComponent)) is NPCComponent component)
                {
                    if (component.interactiveArea.Intersects(Owner.collider))
                    {
                        component.Collisions(Owner);
                    }
                }
            }
        }
    }
}