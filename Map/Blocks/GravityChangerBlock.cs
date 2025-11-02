using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.CustomTiledTypes;
using Juegazo.EntityComponents;
using Juegazo.Map.Components;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class GravityChangerOrbBlock : Block
    {
        public bool changeHorizontal = false;
        public bool changeVertical = false;
        private bool changer = false;
        public GravityChangerOrbBlock() { }
        public GravityChangerOrbBlock(Rectangle collider, bool cH, bool cV) : base(collider)
        {
            changeHorizontal = cH;
            changeVertical = cV;
        }
        public override void Start()
        {
            if (tile == null)
            {
                System.Diagnostics.Debug.WriteLine("Warning: No tile given to block");
                Console.WriteLine($"no tile given to block {this.GetType()}");
                return;
            }
            if(tile.Animation.Count != 0)
            {
                AddComponent(new ChangingBlockComponent());
            }
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (!entity.TryGetComponent(out KeyboardInputComponent keyboardInput)) return;
            if (!(keyboardInput.btnpUp && !changer)) return;
            entity.velocity = new();
            if (changeVertical)
            {
                var cp = entity.direction;
                switch (cp)
                {
                    case FACES.TOP:
                        entity.direction = FACES.BOTTOM;
                        break;
                    case FACES.BOTTOM:
                        entity.direction = FACES.TOP;
                        break;
                    case FACES.LEFT:
                        entity.direction = FACES.TOP;
                        break;
                    case FACES.RIGHT:
                        entity.direction = FACES.BOTTOM;
                        break;
                }
                changer = true;
            }
            if (changeHorizontal)
            {
                var cp = entity.direction;
                switch (cp)
                {
                    case FACES.TOP:
                        entity.direction = FACES.LEFT;
                        break;
                    case FACES.BOTTOM:
                        entity.direction = FACES.RIGHT;
                        break;
                    case FACES.LEFT:
                        entity.direction = FACES.RIGHT;
                        break;
                    case FACES.RIGHT:
                        entity.direction = FACES.LEFT;
                        break;
                }
                changer = true;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collider);
            changer = false;
        }
    }
}