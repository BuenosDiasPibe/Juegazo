using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class DoorBlock : Block
    {
        public bool isOpen = false;
        public uint key { get; protected set; }
        public DoorBlock()
        {
            value = 27;
        }
        public DoorBlock(Rectangle collider) : base(collider)
        { }
        public DoorBlock(Rectangle collider, uint key, bool isOpen) : base(collider)
        {
            Console.WriteLine($"DoorBlock key {key}");
            this.key = key;
            this.isOpen = isOpen;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent(typeof(KeysIDHolderComponent)))
            {
                KeysIDHolderComponent c = (KeysIDHolderComponent)entity.getComponent(typeof(KeysIDHolderComponent));
                if (c.isKeyOnPlayer(key))
                {
                    return;
                }
            }
            new CollisionBlock().horizontalActions(entity, collision);
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent(typeof(KeysIDHolderComponent)))
            {
                KeysIDHolderComponent c = (KeysIDHolderComponent)entity.getComponent(typeof(KeysIDHolderComponent));
                if (c.isKeyOnPlayer(key))
                {
                    return;
                }
            }
            new CollisionBlock().verticalActions(entity, collision);
        }
    }
}