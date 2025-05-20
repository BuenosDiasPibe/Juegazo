using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Juegazo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MarinMol;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Viewport viewport;
    private const int TILESIZE = 64;
    private Player player;
    private KeyboardState prevState;
    private HitboxTilemaps tilemaps;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: add new resolution
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        viewport = GraphicsDevice.Viewport;

        Texture2D worldTexture = Content.Load<Texture2D>("worldTexture");
        tilemaps = new HitboxTilemaps(worldTexture, TILESIZE, 8);

        player = new Player(
            Content.Load<Texture2D>("playerr"),
            new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE), //donde aparece el jugador
            new Rectangle(TILESIZE, TILESIZE * 2, TILESIZE, TILESIZE),
            Color.White
        );
        tilemaps.tilemap = tilemaps.LoadMap("Data/datas.csv"); // cambiar a ../../../Data/datas.csv si causa algun error
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }

        player.Update(gameTime, Keyboard.GetState(), prevState, viewport);
        tilemaps.Update(player, TILESIZE);

        prevState = Keyboard.GetState();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        tilemaps.Draw(_spriteBatch);
        player.DrawSprite(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
