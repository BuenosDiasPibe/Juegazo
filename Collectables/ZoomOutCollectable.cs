using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Collectables
{
    public class ZoomOutCollectable : Collectable
    {
        public ZoomOutCollectable(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
        }

        public override void changeThings(Player player)
        {
            player.zoomOutCamera = true;
        }
    }
}