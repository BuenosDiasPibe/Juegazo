using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class Key : Block //yeah, it doesnt make sense, but wathever
    {
        public uint KeyID { get; set; }
        public Rectangle keyCollider { get; set; }
        public Key()
        {
            KeyID = 0;
            keyCollider = new();
        }
        public Key(uint gotKey, Rectangle keyCollider) : base(keyCollider)
        {
            this.KeyID = gotKey;
            this.keyCollider = keyCollider;
        }
        public Key(Rectangle keyCollider) : base(keyCollider)
        {
            this.keyCollider = keyCollider;
        }

        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            KeysIDHolderComponent entityComponent = new();
            if (!entity.hasComponent(typeof(KeysIDHolderComponent)))
            {
                entity.AddComponent(entityComponent.GetType(), entityComponent);
                Console.WriteLine($"added new key {this.KeyID} to entity");
            }
            entityComponent = (KeysIDHolderComponent)entity.GetComponent(entityComponent.GetType());
            entityComponent.keyHolder.Add(this.KeyID);
            entity.entityState = EntityState.GOT_KEY;

            this.EnableCollisions = false;
            this.enableDraw = false;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}