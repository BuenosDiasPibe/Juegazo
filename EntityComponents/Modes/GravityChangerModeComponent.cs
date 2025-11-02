using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.EntityComponents.Modes
{
    public class GravityChangerMode : Component
    {
        public bool changeHorizontal = false;
        public bool changeVertical = false;
        public GravityChangerMode(CustomTiledTypes.GravityChangerMode c)
        {
            changeHorizontal = c.changeHorizontal;
            changeVertical = c.changeVertical;
        }
        public override void Destroy()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}