using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Collectables
{
    public class ZoomInCollectable : Collectable
    {
        public ZoomInCollectable(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Rectangle collider, Color color) : base(texture, sourceRectangle, Destrectangle, collider, color)
        {
        }

        public override void changeThings(Player player)
        {
            player.zoomInCamera = true;
        }
    }
}