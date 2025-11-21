using MonoGameGum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Juegazo;
using MonoGameGum.Forms.Controls;
using System;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.GueDeriving;

namespace MarinMol.Scenes
{
  public class PauseScene : IScene
  {
    GumService gum;
    SceneManager sceneManager;
    private event Action exitGame;
    public GameplayScene reference;
    private bool addNextButton = false;

    private Texture2D background;
    public PauseScene(GumService gum, SceneManager sceneManager, IScene scene)
    {
      this.gum = gum;
      this.sceneManager = sceneManager;
      this.exitGame = sceneManager.ActionByName["Exit"];
      reference = (GameplayScene)scene;

      background = new Texture2D(sceneManager.graphics.GraphicsDevice, 1, 1);
      background.SetData([Color.Black]);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      reference.Draw(new(), spriteBatch);
      reference.DrawUI(gameTime, spriteBatch);
      spriteBatch.Draw(background, Camera.Instance.ViewPortRectangle, new Color(new Vector4(255,255,255,0.5f)));
    }

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

      TextRuntime text = new()
      {
        Text = "Pause Menu",
        FontScale = 2
      };
      panel.AddChild(text);

      Button but = new()
      { Text = "resume" };
      but.Click += ResumeGame();
      panel.AddChild(but);
      addNextButton = reference != null && !reference.levelPath.Equals("Main.tmj");

      if(addNextButton)
      {
        Button backToMain = new()
        { Text = "Back to Main World" };
        backToMain.Click += BackToMain();
        panel.AddChild(backToMain);
      }
      Button exit = new()
      { Text = "ExitGame" };
      exit.Click += Exiting();

      panel.AddChild(exit);
      gum.Root.AddChild(panel);
    }
    private EventHandler ResumeGame()
    {
      return (sender, e) => {sceneManager.RemoveScene();};
    }
    private EventHandler Exiting()
    {
      return (sender, e) => {exitGame?.Invoke();};
    }
    public EventHandler BackToMain()
    {
      return (sender, e) => { backToMainAction(); };
    }
    public void UnloadContent()
    {
      GumService.Default.Root.Children.Clear();
    }
    bool isReleased = false;

    public void Update(GameTime gameTime)
    {
      if(isReleased && Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        isReleased = false;
        sceneManager.RemoveScene();
      }
      if(addNextButton && isReleased && Keyboard.GetState().IsKeyDown(Keys.M))
      {
        isReleased = false;
        backToMainAction();
        Console.WriteLine("fuck");
      }

      // check if key was pressed before this scene was initialized, if so,
      // dont let it change the scene so quickly
      if(Keyboard.GetState().GetPressedKeyCount() == 0){
        isReleased = true;
      }
    }
    private void backToMainAction()
    {
      reference.levelPath = "Main.tmj";
      reference.UnloadContent();
      sceneManager.RemoveAndLoadLastScene();
    }
  }
}
