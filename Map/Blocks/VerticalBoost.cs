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
            value = 8;
        }
        //TODO: añadir metodos que utilicen isCompleteBlock y toUP
        public VerticalBoostBlock(Rectangle collider, int vertBoost, bool isCompleteBlock, bool toUp) : base(collider)
        {
            value = 8;
            this.vertBoost = vertBoost;
        }
        public VerticalBoostBlock() { value = 8; }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            verticalActions(entity, collision);
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.velocity.Y -= 10;
        }
    }
}