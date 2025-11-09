using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Juegazo.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class SpeedUpBlock : Block
    {
        private int velocitySpeed = 0;
        public SpeedUpBlock(Rectangle collider) : base(collider) { }

        public SpeedUpBlock() { }
        public SpeedUpBlock(Rectangle collider,CustomTiledTypes.SpeedUpBlock sppeedUp) : base(collider)
        {
            this.velocitySpeed = sppeedUp.SpeedAmmount;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            var moveComponent = entity.TryGetComponent(out MoveHorizontalComponent c);
            if (entity.velocity.X != 0)
            {
                entity.velocity.X += entity.directionLeft ? -velocitySpeed : velocitySpeed;
                if(loadedAudio)
                {
                    // randomly play a sound effect from the global SoundManager dictionary
                    var rnd = new Random();
                    int index = rnd.Next(soundEffectsByName.Values.Count);
                    Console.WriteLine($"playing {index} from {soundEffectsByName.Values.Count}");
                    var sfx = soundEffectsByName.ElementAt(index).Value;
                    sfx?.Play();
                }
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
        }
    }
}