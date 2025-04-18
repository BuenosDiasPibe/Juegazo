using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MarinMol;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Viewport viewport;

    private Texture2D sir;
    private Vector2 position;
    private float t;
    private bool coso;

    private bool keypressed;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: add new resolution

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        viewport = GraphicsDevice.Viewport;

        //TODO: create player class
        sir = Content.Load<Texture2D>("player_body");
        // viewport = pantalla visible; sir = el tamaño de la textura; si obtienes la mitad del alto y ancho del viewport tenes el centro de lapantalla, pero desde ese punto iniciara el dibujo del personaje, por lo que debes restarle el alto y ancho del personaje para que se dibuje en el lugar correcto
        position = new Vector2((viewport.Width-sir.Width)/2, (viewport.Height-sir.Height)/2); 
        keypressed = false;
        coso = false; //TODO: add a better name for this, ffs
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
        }
        //TODO: add logic to Player class
        if((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) && !keypressed ) {
            keypressed = true;
            coso = true;
        }
        if(Keyboard.GetState().IsKeyUp(Keys.W) && Keyboard.GetState().IsKeyUp(Keys.Up)) { //TODO: add better check for multiple jump
            keypressed = false;
        }

        if(coso && t <= 1) { // TODO: we dont need linear interpolation for this
            t += 0.5f;
        } else if(t>=1 || t <=-1) { // TODO: add ground check
            coso = false;
            t = 0;
        }
        position.Y = lerp(position.Y, position.Y-50, t);

        if(position.Y < viewport.Height-sir.Height) { // floor
            position.Y += 1; // gravity force
        }

        base.Update(gameTime);
    }

    public float lerp(float a, float b, float t){
        return (1-t)*a + b*t; // linear interpolation function, not really needed
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
    
        _spriteBatch.Draw(sir, position, Color.White);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
