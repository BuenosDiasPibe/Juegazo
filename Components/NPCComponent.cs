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
using FlatRedBall.Glue.StateInterpolation;

namespace Juegazo.Components
{
    public class NPCComponent : Component
    {
        private Camera camera;
        private Rectangle collisionArea = new();
        private int dialogStart = 0;
        private int dialogEnd = 1;
        public string name { get; protected set; }
        public Rectangle interactiveArea { get; protected set; } = new();
        private GumService gum;
        public bool displayBox = false;
        private ColoredRectangleRuntime background;
        private bool displayCreature;
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

            GumService.Default.Root.Children.Clear();
            background = CreateDialogBox(camera.Viewport.Bounds);
            background.AddToRoot();
        }
        private ColoredRectangleRuntime CreateDialogBox(Rectangle rec)
        {
            var background = new ColoredRectangleRuntime();
            background.X = 0;
            background.Y = camera.Viewport.Height / 2;
            background.Width = rec.Width;
            background.Height = rec.Height / 2;
            background.Color = Color.Black;

            return background;
        }
        public override void Destroy()
        {
            Console.WriteLine("hay causita no quiero irme");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (displayBox)
            {
                gum.Draw();
            }
        }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
                if (displayBox)
                {
                    gum.Draw();
                }
        }

        public override void Update(GameTime gameTime)
        {
        }
        private KeyboardState prevState;
        public void Collisions(Entity entity)
        {
            if (interactiveArea.Intersects(entity.Destinationrectangle) && entity.TryGetComponent(out KeyboardInputComponent coso) )
            {
                if (coso.btnpSpecial2)
                {
                    displayBox = !displayBox;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.C) && prevState.IsKeyUp(Keys.C))
                    displayCreature = !displayCreature;
                prevState = Keyboard.GetState();
            }
            else
            {
                displayBox = false;
            }
        }
    }
}
