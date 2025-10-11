using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Juegazo.Map;
using Juegazo.Map.Components;
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
        public bool draw = true;

        public bool EnableUpdate { get; protected set; } = false; //by default no object needs an update method
        public bool EnableCollisions { get; protected set; } = true;

        protected Block() { }
        protected Block(Rectangle collider)
        {
            this.collider = collider;
        }
        protected Block(Tile tile) { this.tile = tile; }
        public virtual void Start()
        {
            if (tile.Animation.Count != 0)
            {
                draw = false;
                AddComponent(new BlockAnimationComponent());
                Console.WriteLine("added animation");
            }
        }
        public virtual void Update(GameTime gameTime) { }
        public abstract void horizontalActions(Entity entity, Rectangle collision);
        public abstract void verticalActions(Entity entity, Rectangle collision);
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            if (draw)
            {
                spriteBatch.Draw(texture, collider, sourceRectangle, Microsoft.Xna.Framework.Color.White);
                return;
            }
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch, texture, sourceRectangle);
            }
        }
        public BlockComponent AddComponent(BlockComponent component)
        {
            if (component.Owner != null)
            {
                throw new Exception("Component " + component + " already has an owner");
            }
            component.Owner = this; 
            component.Start(); 
            this.components.Add(component);
            return component;
        }
    }
}