using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Juegazo;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class CompleteLevelBlock : Block
    {
        public bool changeScene = false;
        public int nextScene = 0;
        public CompleteLevelBlock(Rectangle collider) : base(collider) { }
        public CompleteLevelBlock() {}
        public CompleteLevelBlock(Rectangle collider,CustomTiledTypes.CompleteLevelBlock coso) : base(collider)
        {
            EnableCollisions = coso.isEnabled;
            nextScene = coso.nextLevel;
            if (nextScene != 0)
            {
                colorBlock = new Color(new ColorProvider().GetColorByNumber(nextScene));
            }
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