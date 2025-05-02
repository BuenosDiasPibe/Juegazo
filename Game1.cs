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
    private float scale = 0.3f;
    Player player;
    private KeyboardState prevState;
    private TileMaps tilemaps;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        // TODO: add new resolution
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        viewport = GraphicsDevice.Viewport;

        //TODO: create player class
        Texture2D texture = Content.Load<Texture2D>("player_body");
        Vector2 position = new Vector2((viewport.Width-texture.Width*(scale-0.1f))/2, (viewport.Height-texture.Height*(scale-0.1f))/2);
        player = new Player(texture, position, scale, Color.White);

        Texture2D worldTexture = Content.Load<Texture2D>("worldTexture");
        tilemaps = new TileMaps(worldTexture, 64, 8);
        //tilemaps.tilemap = tilemaps.LoadMap("Data/datas_Collision.csv");
        tilemaps.tilemap = tilemaps.LoadMap("Data/datas.csv");
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
        }
        player.Update(gameTime, Keyboard.GetState(), prevState, viewport);
        prevState = Keyboard.GetState();
        //TODO: add collision detection
        //Logic for moving the player
        base.Update(gameTime);
    }

    public float lerp(float a, float b, float t) {
        return a+(b-a)*t; // linear interpolation function, not really needed
    }
    public float inverseLerp(float a, float b, float value) {
        return (value-a)/(b-a);
    }
    public float mixedFunctions(float fr){ //smoothing thing (its really good i promise)
        float v1 = fr*fr;
        float v2 = 1-(1-fr)*(1-fr);
        return lerp(v1,v2,fr);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(new Vector3(0.1f,0.1f,0.1f)));
        _spriteBatch.Begin(samplerState:SamplerState.PointClamp);

        tilemaps.Draw(_spriteBatch);
        player.DrawSprite(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
