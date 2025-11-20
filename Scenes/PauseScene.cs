using MonoGameGum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Juegazo;
using MonoGameGum.Forms.Controls;
using System;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Input;

namespace MarinMol.Scenes
{
  public class PauseScene : IScene
  {
    GumService gum;
    SceneManager sceneManager;
    private event Action exitGame;
    public PauseScene(GumService gum, SceneManager sceneManager)
    {
      this.gum = gum;
      this.sceneManager = sceneManager;
      exitGame = sceneManager.ActionByName["Exit"];
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    { }

    public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public void Initialize(Game game)
    { }

    public void LoadContent()
    {
      GumService.Default.Root.Children.Clear();
      StackPanel panel = new() {Spacing = 10};
      panel.Anchor(Anchor.Center);
      Button but = new()
      { Text = "resume" };
      but.Click += ResumeGame();
      Button exit = new()
      { Text = "ExitGame" };
      exit.Click += Exiting();

      panel.AddChild(but);
      panel.AddChild(exit);
      gum.Root.AddChild(panel);
    }
    public EventHandler ResumeGame()
    {
      return (sender, e) => {sceneManager.RemoveScene();};
    }
    public EventHandler Exiting()
    {
      return (sender, e) => {exitGame();};
    }
    public void UnloadContent()
    {
      GumService.Default.Root.Children.Clear();
    }
    bool isReleased = false;

    public void Update(GameTime gameTime)
    {
      if(isReleased && Keyboard.GetState().IsKeyDown(Keys.P))
      {
        isReleased = false;
        sceneManager.RemoveScene();
      }
      // check if key was pressed before this scene was initialized, if so,
      // dont let it change the scene so quickly
      if(Keyboard.GetState().IsKeyUp(Keys.P)){isReleased = true;}

    }
  }
}
