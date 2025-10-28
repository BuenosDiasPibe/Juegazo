using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Map.Components;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class MovingDamageBlock : Block
    {
        public CustomTiledTypesImplementation.MovingDamageBlock data;
        private int damageAmmount = 0;
        private Rectangle realCollision = new();
        public MovingDamageBlock() { }
        public MovingDamageBlock(Rectangle collider, CustomTiledTypesImplementation.MovingDamageBlock data) : base(collider)
        {
            this.data = data;
            EnableUpdate = data.data.canMove;
            EnableCollisions = data.data.canDamage;
            realCollision = collider;
            damageAmmount = data.data.damageAmmount;
        }
        public override void Start()
        {
            if (data == null) throw new InvalidOperationException("MovingDamageBlock is set in a non-Object Layer, delete it");
            this.AddComponent(new MovementPingPongBlockComponent(data.InitialBlockPosition, data.endBlockPosition, data.data.velocity));
            base.Start();
        }
        public override void Update(GameTime gameTime)
        {
            foreach(var c in components.Where(c => c.EnableUpdate))
            {
                c.Update(gameTime);
                realCollision.Location = collider.Location;
            }
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if(realCollision.Intersects(entity.collider))
            {
                entity.health -= damageAmmount;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if(realCollision.Intersects(entity.collider))
            {
                entity.health -= damageAmmount;
            }
        }
    }
}