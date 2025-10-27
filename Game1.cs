using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Juegazo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;

namespace MarinMol;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager sceneManager;
    GumService gum => GumService.Default;
    private Camera principalCamera;
    Viewport viewport;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        sceneManager = new();
    }

    protected override void Initialize()
    {
        // TODO: add new resolution
        _graphics.IsFullScreen = true;
        if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1920 &&
            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height >= 1080)
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }
        _graphics.ApplyChanges();
        gum.Initialize(this);

        viewport = GraphicsDevice.Viewport;
        principalCamera = new(viewport.Width, viewport.Height);        

        sceneManager.AddScene(new TitleScene(sceneManager, Content, GraphicsDevice, gum, principalCamera));
        // sceneManager.GetScene().Initialize(this); // only used on TitleScene, its not very useful
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sceneManager.GetScene().LoadContent();
        Console.WriteLine(sceneManager.GetScene()); 
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
        sceneManager.GetScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(30,30,46));
        _spriteBatch.Begin(transformMatrix: principalCamera.Matrix,samplerState: SamplerState.PointWrap); // pointWrap is more useful than pointClamp
        sceneManager.GetScene().Draw(gameTime, _spriteBatch);
        sceneManager.GetScene().DrawUI(gameTime, _spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
