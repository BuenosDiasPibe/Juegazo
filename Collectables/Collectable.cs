using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public abstract class Collectable : Entity
    {
        public Collectable(Texture2D texture,
                            Rectangle sourceRectangle,
                            Rectangle Destrectangle,
                            Rectangle collider,
                            Color color) : base(texture, sourceRectangle, Destrectangle, collider, color)
        { }
        public abstract void changeThings(Player player);
    }
}