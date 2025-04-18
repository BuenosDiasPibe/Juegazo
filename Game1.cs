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
    private float scale = 0.3f;

    private Vector2 objectLeft;
    private Vector2 objectRight;
    private Vector2 fourthObject;
    private bool goLeft;
    private bool goRight;

    private float t;
    private bool coso;
    private bool onGround;
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
        position = new Vector2((viewport.Width-sir.Width*(scale-0.1f))/2, (viewport.Height-sir.Height*(scale-0.1f))/2);

        float howFar = 0.1f; 
        objectLeft = new Vector2(sir.Width*scale, (viewport.Height-sir.Height*scale)/2);
        objectRight = new Vector2(viewport.Width-(sir.Width*scale)*2, (viewport.Height-sir.Height*scale)/2);
        fourthObject = new Vector2(position.X, (viewport.Height+sir.Height*scale)/2);
        goLeft = false;
        goRight = !goLeft;
        t=inverseLerp(objectLeft.X,objectRight.X, position.X);
        Console.WriteLine(t);

        keypressed = false;
        coso = false; //TODO: add a better name for this, ffs
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
        }
        //TODO: add logic to Player class
        //TODO: fix player jump,
            //no debe depender de cuando tiempo el jugador apreta la tecla
            //deberia tener un sistema de fisicas implementado?
        // if(position.Y >= viewport.Height-sir.Height) onGround=true;
        // if((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) && onGround && position.Y > 0) {
        //     if (!keypressed) {
        //         t = position.Y;
        //         keypressed = true;
        //     }
        //     if (position.Y > t - 40) {
        //     position.Y -= 5;
        //     }
        // } else {
        //     keypressed = false;
        //     if(!(position.Y >= viewport.Height-sir.Height)){
        //         onGround=false;
        //         position.Y +=2;
        //     }
        // }
        // //Logic for moving the player
        // if((Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) && position.X != 0){
        //     position.X -= 10;
        // }if((Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) && position.X != viewport.Width-sir.Width){
        //     position.X += 10;
        // }
        if(t>=1){
            goLeft=true;
            goRight = !goLeft;
        }if(t<=0){
            goRight=true;
            goLeft = !goRight;
        }
        if(goRight){
            t +=0.01f;
        }
        if(goLeft){
            t -=0.01f;
        }

        position.X = lerp(objectRight.X, objectLeft.X, mixedFunctions(t));
        fourthObject.X -= (fourthObject.X-position.X)*0.1f;

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
    

        _spriteBatch.Draw(sir, new Rectangle((int)objectLeft.X, (int)objectLeft.Y, (int)(sir.Width*scale), (int)(sir.Height*scale)), new Color(new Vector3(0.9f,0.3f,0.3f)));

        _spriteBatch.Draw(sir, new Rectangle((int)objectRight.X, (int)objectRight.Y, (int)(sir.Width*scale), (int)(sir.Height*scale)), new Color(new Vector3(0.3f,0.3f,0.9f)));

        _spriteBatch.Draw(sir, new Rectangle((int)position.X, (int)position.Y, (int)(sir.Width*(scale-0.1f)), (int)(sir.Height*(scale-0.1f))), Color.White);


        _spriteBatch.Draw(sir, new Rectangle((int)fourthObject.X, (int)fourthObject.Y, (int)(sir.Width*scale), (int)(sir.Height*scale)), new Color(new Vector3(0.5f,0.5f,0.5f)));
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
