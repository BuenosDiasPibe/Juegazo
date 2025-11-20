using System;
using Juegazo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace MarinMol;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager sceneManager;
    private static GumService Gum => GumService.Default;
    Viewport viewport;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1920 &&
            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height >= 1080)
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }
        else if(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1365)
        {
            _graphics.PreferredBackBufferWidth = 1366;
            _graphics.PreferredBackBufferHeight = 768;

        }
        else
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

        }
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        viewport = GraphicsDevice.Viewport;

        Gum.Initialize(this);
        Camera.Initialize(viewport.Width, viewport.Height);
        Debugger.Initialize(_graphics.GraphicsDevice, Color.White);
        sceneManager = new(Content, Gum);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sceneManager.ActionByName["Exit"] = Exit;

        sceneManager.AddScene(new TitleScene(sceneManager, Content, GraphicsDevice, Gum));
        sceneManager.GetScene().LoadContent();
        Console.WriteLine(sceneManager.GetScene()); 
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
        sceneManager.GetScene().Update(gameTime);
        base.Update(gameTime);
        Gum.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      Color selected = new Color(Colorazos.GruvBg);
      GraphicsDevice.Clear(selected);
      _spriteBatch.Begin(transformMatrix: Camera.Instance.Matrix,samplerState: SamplerState.PointWrap); // pointWrap is more useful than pointClamp
      sceneManager.GetScene().Draw(gameTime, _spriteBatch);
      sceneManager.GetScene().DrawUI(gameTime, _spriteBatch);
      Gum.Draw();
      _spriteBatch.End();
      base.Draw(gameTime);
    }
}
