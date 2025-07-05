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

namespace Juegazo
{
    public class TitleScene : IScene
    {
        GumService gum;
        StackPanel panel;
        ContentManager cmanager;
        GraphicsDevice cdevice;
        SceneManager manager;

        public TitleScene(SceneManager manager, ContentManager contentManager, GraphicsDevice graphicsDevice, GumService gum)
        {
            this.manager = manager;
            this.cmanager = contentManager;
            this.cdevice = graphicsDevice;
            this.gum = gum;
            panel = new StackPanel();
            panel.Visual.AddToRoot();
            panel.Spacing = 3;
            panel.Anchor(Anchor.Center);
        }

        public void createShit(Game game)
        {
            Button playButton = new();
            playButton.Text = "Play";
            playButton.Click += StartGame();
            panel.AddChild(playButton);
            Button escGame = new();
            escGame.Text = "exit";
            escGame.Click += (sender, e) => game.Exit();
            panel.AddChild(escGame);
        }

        private EventHandler StartGame()
        {
            return (sender, e) => manager.AddScene(new TestScene(cmanager, cdevice, gum, manager));
        }

        public void drawUI()
        {
            gum.Draw();
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

        public void Initialize(Game game)
        {
            gum.Initialize(game);
            createShit(game);
        }

        public void LoadContent()
        {
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