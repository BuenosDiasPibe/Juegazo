using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class CompleteBlock : BlockType
    {
        public CompleteBlock()
        {
            value = 13;
        }
        public override void horizontalActions(Entity entity, Rectangle collision, int _val)
        {
            if (_val == value)
            {
                Console.WriteLine("Completaste el nivel!");
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision, int _val)
        {
            if (_val == value)
            {
                Console.WriteLine("Completaste el nivel!");
            }
        }
    }
}