/*
    I stole this idea from the Monofoxe Game Engine: https://github.com/Martenfur/Monofoxe
    it was on the awesome monogame list: https://github.com/aloisdeniel/awesome-monogame
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public abstract class Component
    {
        public Entity Owner { get; internal set; }
        public bool EnableUpdate = true; //defaults to true, can do stuff in Update Methods
        public bool EnableDraw = false; //default to false, draw its not visible
        public virtual void Start()
        { }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Destroy();
    }
}