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
        public Collectable(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
        }

        public void changeThings()
        {
            Console.WriteLine("obtuvo coleccionable");
        }

        public override void Update(GameTime gameTime, List<Entity> entities, List<WorldBlock> worldBlocks, List<InteractiveBlock> interactiveBlocks)
        {}
    }
}