using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map
{
    public abstract class BlockComponent
    {
        public Block Owner { get; internal set; }
        public bool EnableUpdate = true; //defaults to true, can do stuff in Update Methods
        public bool EnableDraw = false; //default to false, draw its not visible
        public virtual void Start()
        { }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle);
        public abstract void Destroy();
    }
}