using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class VerticalBoostBlock : Block
    {
        public float vertBoost = 0;
        public VerticalBoostBlock(Rectangle collider) : base(collider) { }
        //TODO: añadir metodos que utilicen isCompleteBlock y toUP
        public VerticalBoostBlock(Rectangle collider, CustomTiledTypes.VerticalBoostBlock vblock) : base(collider)
        {
            vertBoost = vblock.Ammount;
        }
        public VerticalBoostBlock() { }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.baseVelocity = new();
            verticalActions(entity, collision);
        }
        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.velocity.Y = -vertBoost;
        }
    }
}