using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public class Collectable : Entity
    {
        public bool isDestroyed;
        public Collectable(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            this.isDestroyed = true;
        }

        public void changeThings()
        {
            Console.WriteLine("obtuvo coleccionable");
            isDestroyed = false;
        }
        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState, List<Collectable> collectables)
        {
            throw new NotImplementedException();
        }
    }
}