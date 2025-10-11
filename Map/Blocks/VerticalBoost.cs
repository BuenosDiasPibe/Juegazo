using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class VerticalBoostBlock : Block
    {
        public float vertBoost = 14;
        public VerticalBoostBlock(Rectangle collider)
            : base(collider)
        {
            type = "VerticalBoostBlock";
        }
        //TODO: añadir metodos que utilicen isCompleteBlock y toUP
        public VerticalBoostBlock(Rectangle collider, int vertBoost, bool isCompleteBlock, bool toUp, DotTiled.Tile tile) : base(collider)
        {
            this.tile = tile;
            type = "VerticalBoostBlock";
            this.vertBoost = vertBoost;
        }
        public VerticalBoostBlock()
        {
            type = "VerticalBoostBlock";
        }
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