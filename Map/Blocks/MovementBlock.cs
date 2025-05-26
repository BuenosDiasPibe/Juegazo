using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class MovementBlock : BlockType
    {
        public MovementBlock()
        {
            value = 16;
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.X > 0.0f)
            {
                entity.Destinationrectangle.X = collision.Left - entity.Destinationrectangle.Width;
            }
            else if (entity.velocity.X < 0.0f)
            {
                entity.Destinationrectangle.X = collision.Right;
            }
        }
        public override void Update(){}

        public void Update(WorldBlock worldBlock)
        {
            Console.WriteLine(worldBlock.Destinationrectangle.X);
            if (1000 > worldBlock.Destinationrectangle.X)
            {
                worldBlock.Destinationrectangle.X += 10;
            }
            else if (100 < worldBlock.Destinationrectangle.X)
            {
                worldBlock.Destinationrectangle.X -= 200;
            }
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (entity.velocity.Y > 0.0f)
            {
                entity.Destinationrectangle.Y = collision.Top - entity.Destinationrectangle.Height;
                entity.velocity.Y = 1f;
                entity.onGround = true;
            }
            else if (entity.velocity.Y < 0.0f)
            {
                entity.velocity.Y *= 0.1f;
                entity.Destinationrectangle.Y = collision.Bottom;
            }
        }
    }
}