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
    public class DamageBlock : Block
    {
        public override string ToString()
        {
            return $"DamageBlock: Collider={collider}, DamageAmount={damageAmmount}";
        }
        public override void Start()
        {
            base.Start();
        }
        private int damageAmmount = 1;
        public DamageBlock(Rectangle collider, int damageAmmount, bool canDamage) : base(collider)
        {
            this.damageAmmount = damageAmmount;
            EnableUpdate = true;
            EnableCollisions = canDamage;
        }
        //when using this, it always has a collider and a tile
        public DamageBlock(Rectangle collider) : base(collider){}
        public DamageBlock()
        {
            EnableUpdate = true;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if(collision.Intersects(entity.collider))
            {
                entity.baseVelocity = new();
                entity.health-=damageAmmount;
            }
        }

        public override void Update(GameTime gameTime)
        {
            long totalFrames = (long)(gameTime.TotalGameTime.TotalMilliseconds / (1000.0 / 60.0));
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if(collision.Intersects(entity.collider))
            {
                entity.baseVelocity = new();
                entity.health-=damageAmmount;
            }
        }
    }
}