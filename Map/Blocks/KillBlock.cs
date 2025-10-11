using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Juegazo.Map.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class KillBlock : Block
    {
        private int damageAmmount = 1;
        public KillBlock(Rectangle collider, int damageAmmount, DotTiled.Tile tile) : base(collider)
        {
            type = "DamageBlock";
            this.damageAmmount = damageAmmount;
            EnableUpdate = true;
            this.tile = tile;
            AddComponent(new BlockAnimationComponent());
        }
        //when using this, it always has a collider and a tile
        public override void Start()
        {
            AddComponent(new BlockAnimationComponent());
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
        public KillBlock(Rectangle collider) : base(collider){ type = "DamageBlock";}
        public KillBlock()
        {
            type = "DamageBlock";
            EnableUpdate = true;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            entity.baseVelocity = new();
            entity.health-=damageAmmount;
        }

        public override void Update(GameTime gameTime)
        {
            long totalFrames = (long)(gameTime.TotalGameTime.TotalMilliseconds / (1000.0 / 60.0));
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.baseVelocity = new();
            entity.health-=damageAmmount;
        }
        public override void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle)
        {
            foreach(var component in components)
            {
                component.Draw(spriteBatch, texture, sourceRectangle);
            }
        }
    }
}