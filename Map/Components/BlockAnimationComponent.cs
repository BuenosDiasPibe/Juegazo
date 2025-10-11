using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Components
{
    public class BlockAnimationComponent : BlockComponent
    {
        public override void Destroy()
        {
        }
        public override void Start()
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaa");
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            Console.WriteLine("holy shit lois");
            spriteBatch.Draw(texture, Owner.collider, sourceRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}