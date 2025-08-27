using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class CompleteBlock : Block
    {
        public bool changeScene;

        public CompleteBlock(Rectangle collider) : base(collider)
        {
            value = 15;
            changeScene = false;
        }
        public CompleteBlock() { value = 15; }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }
    }
}