using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    List<Sprite> sprites = new List<Sprite>();
    private KeyboardState prevState;
    private float t;
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

        //TODO: create player class
        Texture2D texture = Content.Load<Texture2D>("player_body");
        Vector2 position = new Vector2((viewport.Width-texture.Width*(scale-0.1f))/2, (viewport.Height-texture.Height*(scale-0.1f))/2);

        player = new Player(texture, position, scale, Color.White);

        Vector2 objectLeft = new Vector2(texture.Width*scale, (viewport.Height-texture.Height*scale)/2);
        Vector2 objectRight = new Vector2(viewport.Width-texture.Width*scale, (viewport.Height-texture.Height*scale)/2);
        Vector2 fourthObject = new Vector2(position.X, viewport.Height/2+texture.Height*scale*2);
        sprites =
        [
            new Sprite(texture, objectLeft, scale, new Color(new Vector3(0.9f,0.3f,0.3f))),
            new Sprite(texture, objectRight, scale, new Color(new Vector3(0.3f,0.3f,0.9f))),
            new Sprite(texture, fourthObject, scale, new Color(new Vector3(0.5f,0.5f,0.5f))),
            new Sprite(texture, new Vector2(viewport.Width/2, viewport.Height-texture.Height/2), 1, new Color(new Vector3(0.5f,0.5f,0.5f))),
        ];
   
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
        }
        player.Update(gameTime, Keyboard.GetState(), prevState);
        prevState = Keyboard.GetState();
        //TODO: add collision detection
        //Logic for moving the player
        player.position += player.velocity;

        // if(t>=1){
        //     goLeft=true;
        //     goRight = !goLeft;
        // }if(t<=0){
        //     goRight=true;
        //     goLeft = !goRight;
        // }
        // if(goRight){
        //     t +=0.01f;
        // }
        // if(goLeft){
        //     t -=0.01f;
        // }

        // position.X = lerp(objectRight.X, objectLeft.X, mixedFunctions(t));

        base.Update(gameTime);
    }

    public float lerp(float a, float b, float t){
        return a+(b-a)*t; // linear interpolation function, not really needed
    }
    public float inverseLerp(float a, float b, float value){
        return (value-a)/(b-a);
    }
    public float mixedFunctions(float fr){
        float v1 = fr*fr;
        float v2 = 1-(1-fr)*(1-fr);
        return lerp(v1,v2,fr);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(new Vector3(0.1f,0.1f,0.1f)));
        _spriteBatch.Begin();

        foreach (Sprite sprite in sprites)
        {
            sprite.DrawSprite(_spriteBatch);
        }
        player.DrawSprite(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
