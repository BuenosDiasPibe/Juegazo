using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class Key : Block //yeah, it doesnt make sense, but wathever
    {
        public uint KeyID { get; set; }
        public Rectangle keyCollider { get; set; }
        public Key()
        {
            KeyID = 0;
            type = "Key";
            keyCollider = new();
        }
        public Key(uint gotKey, Rectangle keyCollider, DotTiled.Tile tile) : base(keyCollider)
        {
            this.tile = tile;
            type = "Key";
            this.KeyID = gotKey;
            this.keyCollider = keyCollider;
        }
        public Key(Rectangle keyCollider) : base(keyCollider)
        {
            type = "Key";
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
            entityComponent = (KeysIDHolderComponent)entity.getComponent(entityComponent.GetType());
            entityComponent.keyHolder.Add(this.KeyID);
            this.EnableCollisions = false;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}