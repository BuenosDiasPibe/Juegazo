using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class CompleteBlock : BlockType
    {
        public bool changeScene;
        public CompleteBlock()
        {
            value = 15;
            changeScene = false;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }
    }
}