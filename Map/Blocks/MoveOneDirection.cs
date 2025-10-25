using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class MoveOneDirection : Block
    {
        public Rectangle initialPosition = new();
        public Rectangle EndPosition = new();
        public CustomTiledTypes.MoveOneDirection mblock;
        public Vector2 velToEntity = new();
        public Vector2 lastBlockPosition = new();
        private float time;
        public MoveOneDirection(Rectangle collider, Rectangle initialPos, Rectangle endPos, CustomTiledTypes.MoveOneDirection mblock) : base(collider)
        {
            initialPosition = initialPos;
            EndPosition = endPos;
            this.mblock = mblock;
            this.EnableUpdate = mblock.canMove;
        }
        public MoveOneDirection() { }
        public override void Update(GameTime gameTime)
        {
            Vector2 current = initialPosition.Location.ToVector2();
            Vector2 target = EndPosition.Location.ToVector2();
            float distance = Vector2.Distance(current, target);
            time = (float)(gameTime.TotalGameTime.TotalSeconds * mblock.velocity / distance) % 1;
            Vector2 newPosition = Vector2.Lerp(current, target, time);

            collider.Location = newPosition.ToPoint();
            
            velToEntity = collider.Location.ToVector2() - lastBlockPosition;
            lastBlockPosition = collider.Location.ToVector2();
        }

        public override void horizontalActions(Entity entity, Rectangle collision)
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            new CollisionBlock().verticalActions(entity, collision);
            entity.baseVelocity = velToEntity;
        }
    }
}