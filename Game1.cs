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
    List<Sprite> sprites = new List<Sprite>();
    private KeyboardState prevState;

    private Dictionary<Vector2, int> tilemap;
    private List<Rectangle> textureStore;
    private Dictionary<Vector2, int> LoadMap(string filePath){
        Dictionary<Vector2, int> result = new();
        StreamReader reader = new(filePath);
        string line;
        int y = 0;
        while((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            for(int x = 0; x < parts.Length; x++){
                if(int.TryParse(parts[x], out int value)){
                    if(value > 0){
                        result[new Vector2(x, y)] = value;
                    }
                }
            }
            y++;
        }
        return result;
    }
    private Texture2D worldTexture;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        tilemap = LoadMap("Data/data.csv"); //for some reason this works, but idk why, if it doesnt work for you, change it to ../../../Data/data.csv
        textureStore = new();
        //TODO: add all texture positions
        for(int x = 0; x<13;x++){
            textureStore.Add(new(8*x,0,8,8)); //change if texture grows at y axis, or change it
        }
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
        worldTexture = Content.Load<Texture2D>("worldTexture");
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
        }
        player.Update(gameTime, Keyboard.GetState(), prevState, viewport, sprites);
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

        foreach(var tile in tilemap)
        {
            Rectangle destination = new Rectangle(
            (int)(tile.Key.X*64),
            (int)(tile.Key.Y*64),
            64,
            64
            );
            Rectangle soruceRectangle = textureStore[tile.Value-1];
            _spriteBatch.Draw(worldTexture, destination, soruceRectangle, Color.White);
        }
        player.DrawSprite(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
