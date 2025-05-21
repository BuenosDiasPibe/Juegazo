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
    private const int TILESIZE = 58;
    private List<Entity> entities;
    private KeyboardState prevState;
    private HitboxTilemaps collisionMap;
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
        collisionMap = new HitboxTilemaps(Content.Load<Texture2D>("worldTexture"), TILESIZE, 8);
        entities = new();
        entities.Add(new Player(Content.Load<Texture2D>("playerr"),
                                new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE), 
                                new Rectangle(TILESIZE, TILESIZE * 2, TILESIZE, TILESIZE), //donde aparece el jugador
                                Color.White
        ));
        collisionMap.tilemap = collisionMap.LoadMap("Data/testLevel.csv"); // cambiar a ../../../Data/datas.csv si causa algun error
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }
        foreach (var entity in entities)
        {
            entity.Update(gameTime, Keyboard.GetState(), prevState);
            collisionMap.Update(entity, TILESIZE);
        }
        
        prevState = Keyboard.GetState();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        collisionMap.Draw(_spriteBatch);
        foreach (var entity in entities)
        {
            entity.DrawSprite(_spriteBatch);
        }
        //new Debugger(GraphicsDevice).drawhitboxEntities(_spriteBatch, entities, collisionMap, TILESIZE); // debugging for uuuh idk
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
