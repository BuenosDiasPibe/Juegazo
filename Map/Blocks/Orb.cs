using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class JumpingOrb : Block
    {
        public int jumpAmmount = 0;
        public JumpingOrb() { }
        public JumpingOrb(Rectangle c, CustomTiledTypesImplementation.JumpingOrb orb) : base(c)
        {
            this.jumpAmmount = orb.jumpAmmount;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if(entity.TryGetComponent(out KeyboardInputComponent c) && c.btnpUp)
            {
                entity.velocity.Y = -jumpAmmount;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}