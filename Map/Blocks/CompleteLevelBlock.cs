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
        public bool changeScene;
        public int nextSceneID { get; protected set; } = 0;
        uint a;
        Color color = Color.White;
        public CompleteLevelBlock(Rectangle collider) : base(collider)
        {
            changeScene = false;
        }
        public CompleteLevelBlock() {}
        public CompleteLevelBlock(Rectangle collider, bool isEnabled, int nextLevel) : base(collider)
        {
            EnableCollisions = isEnabled;
            nextSceneID = nextLevel;
            if (nextLevel != 0)
            {
                a = new ColorProvider().GetColorByNumber(nextLevel);
                color = new Color(a);
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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            spriteBatch.Draw(texture, collider, sourceRectangle, color);
        }
    }
}