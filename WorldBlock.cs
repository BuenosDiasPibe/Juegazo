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
        public BlockType blockType { get; private set; }

        public WorldBlock(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color, BlockType blockType) : base(texture, sourceRectangle, Destrectangle, color)
        {
            //asegura de que no usen la misma instancia de movementBlock, que causa que cuando un bloque colisiona con un objeto, todos los bloques con MovementBlock cambien de direccion, que no es lo que necesitamos
            this.blockType = blockType.GetType() == typeof(MovementBlock) ? new MovementBlock() : blockType;
        }

        public void Update()
        {

            if (blockType is MovementBlock movementBlock) // this is equal to blockType.GetType() == typeof(MovementBlock)
            {
                // MovementBlock movementBlock = (MovementBlock)blockType; // only need this if you use the other method
                movementBlock.Update(this);
            }
            blockType.Update();
        }
    }
}