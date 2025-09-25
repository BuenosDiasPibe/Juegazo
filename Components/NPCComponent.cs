using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo.Components
{
    public class NPCComponent : Component
    {
        private Camera camera;
        private Rectangle collisionArea = new();
        private int dialogStart = 0;
        private int dialogEnd = 1;
        private string name;
        public Rectangle interactiveArea { get; protected set; } = new();
        public NPCComponent(Camera camera, string name, int dialogStart, int dialogEnd)
        {
            this.camera = camera;
            this.EnableUpdate = true;
            this.name = name;
            this.dialogStart = dialogStart;
            this.dialogEnd = dialogEnd;
        }
        public override void Start()
        {
            interactiveArea = new Rectangle(
                    Owner.collider.X - Owner.collider.Width,
                    Owner.collider.Y - Owner.collider.Height,
                    Owner.collider.Width * 3, Owner.collider.Height * 3);
        }
        public override void Destroy()
        {
            Console.WriteLine("hay causita no quiero irme");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle r = camera.ViewPortRectangle;
            spriteBatch.Draw(Owner.texture, new Rectangle(
                r.X,
                r.Y+r.Height/2,
                r.Width,
                r.Height/2),
                Owner.sourceRectangle,
                Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
        }
        private KeyboardState prevState;
        public void Collisions(Entity entity)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.X) && prevState.IsKeyUp(Keys.X))
            {
                this.EnableDraw = !EnableDraw;
                Console.WriteLine("yipieeee");
            }
            prevState = Keyboard.GetState();
        }
    }
}
