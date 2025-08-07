using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class JumpCollectable : Collectable
    {
        private int jumpQuantity = 3;

        public JumpCollectable(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        { }

        public override void changeThings(Player player)
        {
            player.incrementJumps += jumpQuantity;
        }
    }
}