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
    public class CompleteBlock : Block
    {
        public bool changeScene;
        public int nextSceneID { get; protected set; } = 0;
        uint a;
        Color color = Color.White;
        public CompleteBlock(Rectangle collider) : base(collider)
        {
            changeScene = false;
            type ="CompleteLevelBlock";
        }
        public CompleteBlock() { type="CompleteLevelBlock"; }
        public CompleteBlock(Rectangle collider, bool isEnabled, int nextLevel, DotTiled.Tile tile) : base(collider)
        {
            this.tile = tile;
            type ="CompleteLevelBlock";
            EnableCollisions = isEnabled;
            nextSceneID = nextLevel;
            a = new ColorProvider().GetColorByNumber(nextLevel);
            color = new Color(a);
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }


        public override void verticalActions(Entity entity, Rectangle collision)
        {
            changeScene = true;
        }
        public override void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            spriteBatch.Draw(texture, collider, sourceRectangle, color);
        }
    }
}