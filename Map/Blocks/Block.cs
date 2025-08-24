using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public abstract class Block : Sprite
    {
        public int value { get; protected set; }
        public Rectangle collider;
        public bool EnableUpdate { get; protected set; } = false; //by default no object needs an update method
        public bool EnableCollisions { get; protected set; } = true;
        protected Block(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Rectangle collider, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            this.collider = collider;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void horizontalActions(Entity entity, Rectangle collision);
        public abstract void verticalActions(Entity entity, Rectangle collision);
    }
}