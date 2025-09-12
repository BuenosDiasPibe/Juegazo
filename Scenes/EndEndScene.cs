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
        ContentManager Content = contentManager;
        GraphicsDevice graphicsDevice = graphicsDevice;
        SceneManager sceneManager = manager;
        Camera principalCamera = camera;

        public void donmt()
        {
            GumService.Default.Root.Children.Clear();

            StackPanel panel = new();
            panel.AddToRoot();
            panel.Spacing = 3;
            panel.Anchor(Anchor.Center);
            TextRuntime end = new();
            end.Text = "game finished!";
            Button retryButton = new()
            {
                Text = "Retry"
            };
            retryButton.Click += RetryGame();
            Button MainMenu = new()
            {
                Text = "Main Menu"
            };
            MainMenu.Click += MenuMain();
            panel.AddChild(end);
            panel.AddChild(retryButton);
            panel.AddChild(MainMenu);
        }
        public void UnloadContent()
        {
            GumService.Default.Root.Children.Clear();
        }
        private EventHandler RetryGame()
        {
            return (sender, e) => sceneManager.RemoveScene(); 
        }
        private EventHandler MenuMain()
        { return (sender, e) => { sceneManager.AddScene(new TitleScene(sceneManager, Content, graphicsDevice, gum, principalCamera)); }; }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public void Initialize(Game game)
        { gum.Initialize(game); }

        public void LoadContent()
        { donmt(); }

        public void Update(GameTime gameTime)
        { gum.Update(gameTime); }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        { gum.Draw(); }
    }
}