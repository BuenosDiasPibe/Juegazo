using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class WaterBlock : Block
    {
        private WaterComponent waterComponent = new();
        private bool canLoad = true;
        public WaterBlock() 
        {
            type = "WaterBlock"; 
        }
        public WaterBlock(Rectangle Collider,bool canLoad) : base(Collider)
        {
            type = "WaterBlock";
            this.canLoad = canLoad;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (!entity.hasComponent(waterComponent.GetType()))
            {
                entity.AddComponent(waterComponent.GetType(), new WaterComponent());
            }
            else
            {
                WaterComponent c = (WaterComponent)entity.GetComponent(waterComponent.GetType());
                c.EnableUpdate = true;
            }
            entity.entityState = EntityState.SWIMMING;
            entity.touchingWaterBlock = true;
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}