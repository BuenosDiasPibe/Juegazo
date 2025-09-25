using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gum.Wireframe;
using MonoGameGum.Forms.Controls;
using MonoGameGum;
using MonoGameGum.GueDeriving;

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
        private GumService gum;
        public bool displayBox = false;
        public NPCComponent(Camera camera, string name, int dialogStart, int dialogEnd, GumService gum)
        {
            this.camera = camera;
            this.EnableUpdate = true;
            this.name = name;
            this.dialogStart = dialogStart;
            this.dialogEnd = dialogEnd;
            this.gum = gum;
            this.EnableDraw = true;

        }
        public override void Start()
        {
            interactiveArea = new Rectangle(
                    Owner.collider.X - Owner.collider.Width,
                    Owner.collider.Y - Owner.collider.Height,
                    Owner.collider.Width * 3, Owner.collider.Height * 3);
        }
        private void CreateDialogBox(Rectangle rec) //TODO: this doesnt work
        {
            GumService.Default.Root.Children.Clear();
            GraphicalUiElement container = new();
            container.SetProperty("Width", (float)rec.Width);
            container.SetProperty("Height", (float)(rec.Y + rec.Height/2));
            container.SetProperty("Background", Color.Red);
            container.SetProperty("ForeGround", Color.White);
            container.SetProperty("X", (float)rec.X);
            container.SetProperty("Y", (float)(rec.Height/2));

            var textDisplay = new TextRuntime();
            textDisplay.SetProperty("Width", 380f);
            textDisplay.SetProperty("Height", 180f);
            textDisplay.SetProperty("X", 10f);
            textDisplay.SetProperty("Y", 10f);
            textDisplay.SetProperty("Text", $"Hello, I am {name}");

            container.AddToRoot();
        }
        public override void Destroy()
        {
            Console.WriteLine("hay causita no quiero irme");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle srcRect = Owner.sourceRectangle;
            if (Owner.getComponent(typeof(AnimationComponent)) is AnimationComponent component)
            {
                srcRect = component.sourceRectangle;
            }
            Rectangle r = camera.ViewPortRectangle;
            // CreateDialogBox(r);
            if (displayBox)
            {
                CreateDialogBox(r);
                spriteBatch.Draw(Owner.texture, new Rectangle(r.X, r.Y + r.Height / 2, r.Width, r.Height / 2), srcRect, Color.Red);
                gum.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
        private KeyboardState prevState;
        public void Collisions(Entity entity)
        {
            if (interactiveArea.Intersects(entity.Destinationrectangle))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.X) && prevState.IsKeyUp(Keys.X))
                {
                    displayBox = !displayBox;
                }
                prevState = Keyboard.GetState();
            }
            else
            {
                displayBox = false;
            }
        }
    }
}
