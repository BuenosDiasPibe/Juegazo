using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.EntityComponents.Modes
{
    public class GravityChangerMode : Component
    {
      Entity lastOwner;
        public bool changeHorizontal = false;
        public bool changeVertical = false;
        public GravityChangerMode(CustomTiledTypes.GravityChangerMode c)
        {
            changeHorizontal = c.changeHorizontal;
            changeVertical = c.changeVertical;
        }
        public override void Destroy()
        { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
          if(lastOwner != Owner)
          {
            Owner.color = new(Colorazos.GruvGay);
          }
          if(Owner.TryGetComponent(out KeyboardInputComponent ke) &&
              ke.btnpSpecial1)
          {
            if(changeHorizontal)
            {
              if(Owner.direction != FACES.LEFT)
              {
                Owner.direction = FACES.LEFT;
              }else if(Owner.direction != FACES.RIGHT)
              {
                Owner.direction = FACES.RIGHT;
              }
            }
            if(changeVertical)
            {
              if(Owner.direction != FACES.TOP)
              {
                Owner.direction = FACES.TOP;
              }else if(Owner.direction != FACES.BOTTOM)
              {
                Owner.direction = FACES.BOTTOM;
              }
            }
          }
        }
    }
}
