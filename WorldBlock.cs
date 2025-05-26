using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Map.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class WorldBlock : Sprite
    {
        public BlockType blockType;

        public WorldBlock(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color, BlockType blockType) : base(texture, sourceRectangle, Destrectangle, color)
        {
            this.blockType = blockType;
        }

        public void Update()
        {
            if (blockType.GetType() == typeof(MovementBlock))
            {
                MovementBlock movementBlock = (MovementBlock)blockType;
                movementBlock.Update(this);
            }
            blockType.Update();
        }
    }
}