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
                if (entity.hasComponent(typeof(NPCComponent))) {
                    if (entity.GetComponent(typeof(NPCComponent)) is NPCComponent component)
                    {
                        component.Collisions(Owner);
                    }
                }
                if (entity.hasComponent(typeof(DoubleJumpComponent)))
                {
                    if (entity.GetComponent(typeof(DoubleJumpComponent)) is DoubleJumpComponent doubleJumpComponent)
                    {
                        doubleJumpComponent.Collisions(Owner);
                    }
                }
            }
        }
    }
}