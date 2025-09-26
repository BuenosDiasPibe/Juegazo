using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;

namespace Juegazo
{
    public class TitleScene : IScene
    {
        public TitleScene(SceneManager manager, ContentManager contentManafger, GraphicsDevice gdevice, GumService gum, Camera camera)
        {
            this.manager = manager;
            this.gum = gum;
            this.camera = camera;
            cmanager = contentManafger;
            graphicsDevice = gdevice;
        }
        GumService gum;
        ContentManager cmanager;
        GraphicsDevice graphicsDevice;
        SceneManager manager;
        Camera camera;

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
            name.Text = "Marin Mol";
            name.FontScale = 2;
            name.Red = 243;
            name.Green = 139;
            name.Blue = 168;
            name.Anchor(Anchor.Top);
            TextRuntime description = new();
            description.Text = "we count the pixels";
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
            return (sender, e) => manager.AddScene(new GameplayScene(cmanager, graphicsDevice, gum, manager, camera));
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public void Initialize(Game game)
        {
        }

        public void LoadContent()
        {
            CreateShit();
        }

        public void UnloadContent()
        { }

        public void Update(GameTime gameTime)
        {
            gum.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                manager.AddScene(new GameplayScene(cmanager, graphicsDevice, gum, manager, camera));
        }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        { gum.Draw(); }
    }
}