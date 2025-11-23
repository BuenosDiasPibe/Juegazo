using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gum.Wireframe;
using MarinMol.Scenes;
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
        GumService gum;
        ContentManager cmanager;
        GraphicsDevice graphicsDevice;
        SceneManager manager;
        private event Action Exit;
        public TitleScene(SceneManager manager, ContentManager contentManafger, GraphicsDevice gdevice)
        {
            this.manager = manager;
            this.gum = GumService.Default;
            cmanager = contentManafger;
            graphicsDevice = gdevice;
            Exit = manager.ActionByName["Exit"];
        }

        public void CreateShit()
        {
            GumService.Default.Root.Children.Clear();

            StackPanel panel = new()
            {
              Spacing = 3,
            };
            panel.Anchor(Anchor.Center);
            TextRuntime name = new()
            {
              Text = "Marin Mol",
              FontSize = 90,
              FontScale = 4,
              IsBold = true,
              UseFontSmoothing = true,
            };
            name.Color = new(Colorazos.GruvPurple);
            TextRuntime description = new()
            {
              Text = "we count the pixels",
              UseFontSmoothing = true
            };
            Button playButton = new()
            { Text = "Play" };
            playButton.Click += StartGame();
            Button ExitButton = new()
            { Text = "Exit" };
            ExitButton.Click += Exiting();

            panel.AddChild(name);
            panel.AddChild(description);
            panel.AddChild(playButton);
            panel.AddChild(ExitButton);
            gum.Root.AddChild(panel);
        }

        private EventHandler StartGame()
        {
          return (_, _) => {
            GameplayScene gs = new GameplayScene(cmanager, graphicsDevice, manager);
            gs.levelPath = "";
            manager.AddScene(gs);
            };
        }
        private EventHandler Exiting()
        {
          return (sender, e) => {Exit();};
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
                manager.AddScene(new GameplayScene(cmanager, graphicsDevice,  manager));
        }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        { }
    }
}
