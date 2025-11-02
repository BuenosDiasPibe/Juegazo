using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class Key : Block //yeah, it doesnt make sense, but wathever
    {
        public uint DoorID;
        public bool isCollected = false;
        public Key()
        { }
        public Key(uint DoorID, Rectangle keyCollider, bool isCollected) : base(keyCollider)
        {
            this.isCollected = isCollected;
            this.DoorID = DoorID;
        }
        public Key(Rectangle keyCollider) : base(keyCollider)
        { }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.entityState = EntityState.GOT_KEY;
            isCollected = true;

            this.EnableCollisions = false;
            this.enableDraw = false;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}