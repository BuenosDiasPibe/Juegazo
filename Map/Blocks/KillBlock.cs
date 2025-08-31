using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class KillBlock : Block
    {
        private int damageAmmount = 1;
        public KillBlock(Rectangle collider, int damageAmmount)
            : base(collider)
        {
            value = 18;
            this.damageAmmount = damageAmmount;
        }
        public KillBlock(Rectangle collider) : base(collider)
        {
            
        }
        public KillBlock() { value = 18; }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            Console.WriteLine($"health: {entity.health}");
            entity.health--;
        }

        public override void Update(GameTime gameTime)
        { }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            entity.health--;
        }
    }
}