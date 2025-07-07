using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;

namespace Juegazo
{
    public class TitleScene(SceneManager manager, ContentManager contentManager, GraphicsDevice graphicsDevice, GumService gum) : IScene
    {
        GumService gum = gum;
        ContentManager cmanager = contentManager;
        GraphicsDevice cdevice = graphicsDevice;
        SceneManager manager = manager;

        public void CreateShit()
        {
            GumService.Default.Root.Children.Clear();

            StackPanel panel = new()
            {
                Spacing = 3
            };
            panel.Anchor(Anchor.Center);
            panel.AddToRoot();
            TextRuntime name = new();
            name.Text = "MARIN MOL";
            name.FontScale = 2;
            name.Red = 243;
            name.Green = 139;
            name.Blue = 168;
            name.Anchor(Anchor.Top);
            TextRuntime description = new();
            description.Text = "we ballin so hawd!!!";
            description.Height = 20;
            Button playButton = new()
            {
                Text = "Play"
            };
            playButton.Click += StartGame();
            playButton.Anchor(Anchor.Center);
            panel.AddChild(name);
            panel.AddChild(description);
            panel.AddChild(playButton);
        }

        private EventHandler StartGame()
        {
            return (sender, e) => manager.AddScene(new TestScene(cmanager, cdevice, gum, manager));
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gum.Draw();
        }

        public void Initialize(Game game)
        {
            gum.Initialize(game);
            CreateShit();
        }

        public void LoadContent()
        {
            Console.WriteLine("First screen");
            if (manager.hasScenes())
            {
                CreateShit();
            }
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            gum.Update(gameTime);
        }
    }
}