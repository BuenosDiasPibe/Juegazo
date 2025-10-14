using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class NPCAnimationComponent : Component
    {
        protected Tile tile;
        private Rectangle sourceRectangle = new();
        //TODO: add state machines for animation states
        public NPCAnimationComponent(Tile tile)
        {
            this.tile = tile;
            this.EnableDraw = tile.Animation != null && tile.Animation.Count != 0;
            this.EnableUpdate = true;
        }
        public override void Start()
        {
            sourceRectangle = new(0,0,Owner.sourceRectangle.Width, Owner.sourceRectangle.Height);
        }
        public override void Destroy()
        {
            Console.WriteLine("se nos va el causita pipipi");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (tile.Width != 0) Console.WriteLine("Special case!!");

            double totalDuration = tile.Animation.Sum(a => a.Duration);
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds % totalDuration;
            int frame = 0;
            double accumulatedTime = 0;

            for (int i = 0; i < tile.Animation.Count; i++)
            {
                accumulatedTime += tile.Animation[i].Duration;
                if (currentTime <= accumulatedTime)
                {
                    frame = i;
                    break;
                }
            }
            NPCComponent c = (NPCComponent)Owner.GetComponent(typeof(NPCComponent));
            var currentFrame = tile.Animation[frame];
            sourceRectangle.X = (int)(currentFrame.TileID % (Owner.texture.Width / Owner.sourceRectangle.Width) * Owner.sourceRectangle.Width);
            sourceRectangle.Y = (int)(currentFrame.TileID / (Owner.texture.Width / Owner.sourceRectangle.Width) * Owner.sourceRectangle.Height);

            spriteBatch.Draw(Owner.texture, Owner.Destinationrectangle, sourceRectangle, Microsoft.Xna.Framework.Color.White);
        }

        public override void Update(GameTime gameTime)
        { }
    }
}