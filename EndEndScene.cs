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
    public class EndScene(SceneManager manager, ContentManager contentManager, GraphicsDevice graphicsDevice, GumService gum, Camera camera) : IScene
    {
        GumService gum = gum;
        ContentManager cmanager = contentManager;
        GraphicsDevice cdevice = graphicsDevice;
        SceneManager manager = manager;
        Camera camera = camera;

        public void donmt()
        {
            GumService.Default.Root.Children.Clear();

            StackPanel panel = new();
            panel.AddToRoot();
            panel.Spacing = 3;
            panel.Anchor(Anchor.Center);
            TextRuntime end = new();
            end.Text = "game finished!";
            Button playButton = new()
            {
                Text = "Retry"
            };
            playButton.Click += StartGame();
            Button MainMenu = new()
            {
                Text = "Main Menu"
            };
            MainMenu.Click += MenuMain();
            panel.AddChild(end);
            panel.AddChild(playButton);
            panel.AddChild(MainMenu);
        }

        private EventHandler StartGame()
        { return (sender, e) => manager.AddScene(new TestScene(cmanager, cdevice, gum, manager, camera)); }
        private EventHandler MenuMain()
        { return (sender, e) => { manager.AddScene(new TitleScene(manager, cmanager, cdevice, gum, camera)); }; }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public void Initialize(Game game)
        { gum.Initialize(game); }

        public void LoadContent()
        { donmt(); }
        public void UnloadContent()
        { }

        public void Update(GameTime gameTime)
        { gum.Update(gameTime); }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        { gum.Draw(); }
    }
}