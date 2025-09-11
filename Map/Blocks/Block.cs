using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public abstract class Block
    {
        public int value { get; protected set; }
        public Rectangle collider = new();
        public bool EnableUpdate { get; protected set; } = false; //by default no object needs an update method
        public bool EnableCollisions { get; protected set; } = true;
        public bool isVisible { get; protected set; } = true; //probably i need to wrap all the bool variables on a struct, 'casuse this is a nightmare
        protected Block() { }
        protected Block(Rectangle collider)
        {
            this.collider = collider;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void horizontalActions(Entity entity, Rectangle collision);
        public abstract void verticalActions(Entity entity, Rectangle collision);
        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            if (!isVisible)
            {
                return;
            }
            spriteBatch.Draw(texture, collider, sourceRectangle, Color.White);
        }
    }
}