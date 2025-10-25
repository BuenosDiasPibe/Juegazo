using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Map.Blocks
{
    public class CheckPointBlock : Block
    {
        public Vector2 position = new();
        public CustomTiledTypes.CheckPointBlock cpb;
        public CheckPointBlock(Rectangle collisionRectangle) : base(collisionRectangle)
        { position = new(collider.X, collider.Y); }
        public CheckPointBlock() { }
        public override void Start()
        {
            if (position == Vector2.Zero)
                position = collider.Location.ToVector2();
            base.Start();
        }
        public CheckPointBlock(Rectangle collision, CustomTiledTypes.CheckPointBlock cpb, Vector2 position) : base(collision)
        {
            this.cpb = cpb;
            EnableCollisions = cpb.isEnabled;
            this.position = position;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.hasComponent<CanDieComponent>())
            {
                if(entity.TryGetComponent(out CanDieComponent canDieComponent))
                {
                    canDieComponent.initialPosition = position;
                }
            }
        }
        public override void verticalActions(Entity entity, Rectangle collision)
        {
            horizontalActions(entity, collision);
        }
    }
}