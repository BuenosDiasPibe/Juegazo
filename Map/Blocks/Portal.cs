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
        public double elapsedTime = 0;
        public int boost = 10;
        private Point positionerBuffer = new();
        public bool canTeleport = false;
        public bool isTeleportingHor = false;
        public bool isTeleportingVer = false;
        public Portal portalLink;
        public Portal() {}
        public Portal(Rectangle rectanglele, uint portalNeeded,  uint portalID, float changeTimeSeconds, bool canTeleport) : base(rectanglele)
        {
            Console.WriteLine($"portal: linkID: {portalNeeded}, delay: {changeTimeSeconds}s, canTeleport: {canTeleport}");
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
            EnableUpdate = true;
        }
        public override void Start()
        {
            if (portalLink == null) throw new Exception("no portal given, this is bad!!!!");
            base.Start();
        }
        public override void Update(GameTime gameTime)
        {
            isTeleportingHor = false;
            isTeleportingVer = false;
            if(canTeleport && !EnableCollisions)
            {
                var elapsed = gameTime.ElapsedGameTime.TotalSeconds;
                colorBlock = Color.Gray;
                portalLink.colorBlock = Color.Gray;
                if (elapsedTime > 0.0f)
                {
                    elapsedTime -= elapsed;
                }
                else if (elapsedTime < 0)
                {
                    colorBlock = Color.White;
                    portalLink.colorBlock = Color.White;
                    EnableCollisions = true;
                    elapsedTime = delayTimeSeconds;
                }
            }
        }
        public override void horizontalActions(Entity entity, Rectangle collision)
        {
            if (collision.Left > entity.Destinationrectangle.Right)
            {
                positionerBuffer.Y = portalLink.collider.X + portalLink.collider.Width;
                isTeleportingHor = true;
            }
            else if (collision.Right > entity.Destinationrectangle.Left)
            {
                positionerBuffer.X = portalLink.collider.X - entity.Destinationrectangle.Width;
                isTeleportingHor = true;
            }
        }
        public override void verticalActions(Entity entity, Rectangle collision)
        {
            if (collision.Bottom > entity.Destinationrectangle.Top)
            {
                positionerBuffer.Y = portalLink.collider.Y + entity.Destinationrectangle.Height;
                isTeleportingVer = true;
            }
            else if (collision.Top > entity.Destinationrectangle.Bottom)
            {
                positionerBuffer.Y = portalLink.collider.Bottom + entity.Destinationrectangle.Height;
                isTeleportingVer = true;
            }
            EnableCollisions = false;
            portalLink.EnableCollisions = false;
            positionEntity(entity);
        }
        private void positionEntity(Entity entity)
        {
            Console.WriteLine("ayyy");
            switch(isTeleportingHor , isTeleportingVer)
            {
                case (true, true):
                    entity.Destinationrectangle.Location = positionerBuffer;
                    Console.WriteLine($"moving all");
                    break;
                case (false, true):
                    entity.Destinationrectangle.Location = new(portalLink.collider.X, positionerBuffer.Y);
                    Console.WriteLine("moving ver");
                    break;
                case (true, false):
                    entity.Destinationrectangle.Location = new(positionerBuffer.X, portalLink.collider.Y);
                    Console.WriteLine("moving hor");
                    break;
            }
        }
    }
}