using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public abstract class BlockType
    {
        public int value { get; protected set; }
        public abstract void horizontalActions(Player player, Rectangle collision, int _val);
        public abstract void verticalActions(Player player, Rectangle collision, int _val);
    }
}