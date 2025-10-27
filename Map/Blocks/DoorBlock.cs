using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class DoorBlock : Block
    {
        public bool isOpen = false;
        public uint ID = 0;
        public List<Key> keys = new();
        public DoorBlock() { }
        public DoorBlock(Rectangle collider) : base(collider) { }
        public DoorBlock(Rectangle collider, bool isOpen, uint ID) : base(collider)
        {
            this.isOpen = isOpen;
            this.ID = ID;
        }
        public override void Start()
        {
            if (keys.Count == 0) throw new Exception("no keys in block");
            base.Start();
            EnableUpdate = true;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if(!(keys.Count > 0 && keys.All(k => k.isCollected)))
                new CollisionBlock().horizontalActions(entity, collision);
        }
        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if(!(keys.Count > 0 && keys.All(k => k.isCollected)))
                new CollisionBlock().verticalActions(entity, collision);
        }
        public override void Update(GameTime gameTime)
        {
            if(keys.Count > 0 && keys.All(k => k.isCollected))
            {
                colorBlock = Color.Black;
            }
        }
    }
}