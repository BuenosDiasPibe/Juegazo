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
            type = "DoorBlock";
        }
        public DoorBlock(Rectangle collider) : base(collider)
        {
            type = "DoorBlock";
        }
        public DoorBlock(Rectangle collider, uint key, bool isOpen, DotTiled.Tile tile) : base(collider)
        {
            this.tile = tile;
            type = "DoorBlock";
            Console.WriteLine($"DoorBlock key {key}");
            this.key = key;
            this.isOpen = isOpen;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent(typeof(KeysIDHolderComponent)))
            {
                KeysIDHolderComponent c = (KeysIDHolderComponent)entity.GetComponent(typeof(KeysIDHolderComponent));
                if (c.isKeyOnPlayer(key))
                {
                    return;
                }
            }
            new CollisionBlock().horizontalActions(entity, collision);
        }


        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent(typeof(KeysIDHolderComponent)))
            {
                KeysIDHolderComponent c = (KeysIDHolderComponent)entity.GetComponent(typeof(KeysIDHolderComponent));
                if (c.isKeyOnPlayer(key))
                {
                    return;
                }
            }
            new CollisionBlock().verticalActions(entity, collision);
        }
    }
}