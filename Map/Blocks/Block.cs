using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Juegazo.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public abstract class Block
    {
        public string type = "";
        public Tile tile;
        public Rectangle collider = new();
        public List<BlockComponent> components = new();

        public bool EnableUpdate { get; protected set; } = false; //by default no object needs an update method
        public bool EnableCollisions { get; protected set; } = true;

        protected Block() { }
        protected Block(Rectangle collider)
        {
            this.collider = collider;
        }
        protected Block(Tile tile) { this.tile = tile; }
        public virtual void Start() { }
        public virtual void Update(GameTime gameTime) { }
        public abstract void horizontalActions(Entity entity, Rectangle collision);
        public abstract void verticalActions(Entity entity, Rectangle collision);
        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            foreach (var component in components)
            {
                component.Draw(spriteBatch, texture, sourceRectangle);
            }

            spriteBatch.Draw(texture, collider, sourceRectangle, Microsoft.Xna.Framework.Color.White);
        }
    }
}