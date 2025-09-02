using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class CompleteBlock : Block
    {
        public bool changeScene;
        public int nextSceneID = 0;

        public CompleteBlock(Rectangle collider) : base(collider)
        {
            value = 15;
            changeScene = false;
        }
        public CompleteBlock() { value = 15; }
        public CompleteBlock(Rectangle collider, bool isEnabled, int nextLevel) : base(collider)
        {
            value = 15;
            EnableCollisions = isEnabled;
            nextSceneID = nextLevel;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }
    }
}