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
        public Tile tile;
        public Rectangle collider = new();
        public Rectangle DestinationRectangle = new();
        public List<BlockComponent> components = new();
        public bool enableDraw = true;

        public bool EnableUpdate { get; protected set; } = false;
        public bool EnableCollisions { get; protected set; } = true;

        protected Block() { }
        protected Block(Rectangle collider)
        {
            this.collider = collider;
        }
        protected Block(Rectangle collider, Rectangle DestinationRectangle)
        {
            this.collider = collider;
            this.DestinationRectangle = DestinationRectangle;
        }
        /// <summary> 
        /// Checker for blocks if they dont have some variable setted up
        /// </summary>
        public virtual void Start()
        {
            if (tile == null)
            {
                System.Diagnostics.Debug.WriteLine("Warning: No tile given to block");
                Console.WriteLine($"no tile given to block {this.GetType()}");
                return;
            }
            if (tile.Animation.Count != 0)
            {
                AddComponent(new BlockAnimationComponent());
            }
        }
        public virtual void Update(GameTime gameTime) { }
        public abstract void horizontalActions(Entity entity, Rectangle collision);
        public abstract void verticalActions(Entity entity, Rectangle collision);
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            if (!enableDraw) return;
            bool chuco = false;
            foreach (var component in components.Where(component => component.EnableDraw))
            {
                component.Draw(gameTime, spriteBatch, texture, sourceRectangle);
                chuco = true;
            }
            if (!chuco)
            {
                spriteBatch.Draw(texture, collider, sourceRectangle, Microsoft.Xna.Framework.Color.White);
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