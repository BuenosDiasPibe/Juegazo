using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo.Map.Blocks
{
    public class Portal : Block
    {
        public uint portalID;
        public uint portalNeeded;
        public float delayTimeSeconds = 0;
        public float elapsedTime = 0;
        public int boost = 10;
        public bool canTeleport = false;
        public Portal portalLink;
        public Portal() {}
        public Portal(Rectangle rectanglele, uint portalNeeded,  uint portalID, float changeTimeSeconds, bool canTeleport) : base(rectanglele)
        {
            this.portalID = portalID;
            this.delayTimeSeconds = changeTimeSeconds;
            elapsedTime = changeTimeSeconds;
            this.canTeleport = canTeleport;
            this.portalNeeded = portalNeeded;
            EnableUpdate = true;
        }
        public Portal(Rectangle rectanglele, Portal portalLink) : base(rectanglele)
        {
            this.portalLink = portalLink;
        }
        public override void Start()
        {
            if (portalLink == null) Console.WriteLine("no portal given, this is bad!!!!");
            base.Start();
        }
        public override void Update(GameTime gameTime)
        {
            if(canTeleport && (!EnableCollisions || !portalLink.EnableCollisions))
            {
                colorBlock = Color.Gray;
                portalLink.colorBlock = Color.Gray;
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (elapsedTime > 0f)
                {
                    elapsedTime -= elapsed;
                }
                if (elapsedTime <= 0f)
                {
                    colorBlock = Color.White;
                    portalLink.colorBlock = Color.White;
                    EnableCollisions = true;
                    portalLink.EnableCollisions = true;
                    elapsedTime = delayTimeSeconds;
                }
            }
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
        }

        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (collision.Bottom > entity.Destinationrectangle.Top)
            {
                entity.Destinationrectangle.Location = new(portalLink.collider.X, portalLink.collider.Top - entity.Destinationrectangle.Height - boost);
            }
            EnableCollisions = false;
            portalLink.EnableCollisions = false;
        }
    }
}