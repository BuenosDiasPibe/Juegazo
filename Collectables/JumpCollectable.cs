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
        private Texture2D t;
        private Rectangle s;
        private Rectangle d;
        private Color c;


        public JumpCollectable(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Rectangle collider, Color color) : base(texture, sourceRectangle, Destrectangle, collider, color)
        {
        }

        public override void changeThings(Player player)
        {
            player.incrementJumps += jumpQuantity;
        }
    }
}