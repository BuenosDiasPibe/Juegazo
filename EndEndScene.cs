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
    public class EndScene : IScene
    {
        GumService gum;
        ContentManager cmanager;
        GraphicsDevice cdevice;
        SceneManager manager;

        public EndScene(SceneManager manager, ContentManager contentManager, GraphicsDevice graphicsDevice, GumService gum)
        {
            this.manager = manager;
            this.cmanager = contentManager;
            this.cdevice = graphicsDevice;
            this.gum = gum;
        }

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
        {
            return (sender, e) => manager.AddScene(new TestScene(cmanager, cdevice, gum, manager));
        }
        private EventHandler MenuMain()
        {
            return (sender, e) => {manager.AddScene(new TitleScene(manager, cmanager, cdevice, gum));};
        }
        public void drawUI()
        {
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gum.Draw();
            spriteBatch.End();
        }

        public void Initialize(Game game)
        {
            gum.Initialize(game);
        }

        public void LoadContent()
        {
            donmt();
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