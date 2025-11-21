using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class DamageBlock : Block
    {
        private int damageAmmount = 1;

        public override string ToString()
        { return $"DamageBlock: Collider={collider}, DamageAmount={damageAmmount}"; }

        public override void Start()
        { base.Start(); }

        public DamageBlock(Rectangle collider, int damageAmmount, bool canDamage) 
          : base(collider)
        {
            this.damageAmmount = damageAmmount;
            EnableUpdate = true;
            EnableCollisions = canDamage;
        }
        //when using this, it always has a collider and a tile
        public DamageBlock(Rectangle collider) : base(collider){}

        public DamageBlock()
        { }

        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if(collision.Intersects(entity.collider))
            {
                entity.baseVelocity = new();
                entity.health-=damageAmmount;
                if(loadedAudio)
                {
                    var rnd = new Random();
                    int index = rnd.Next(soundEffectsByName.Values.Count);
                    var sfx = soundEffectsByName.ElementAt(index).Value;
                    sfx?.Play();
                }
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}
