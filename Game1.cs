using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Juegazo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MarinMol;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Viewport viewport;
    private const int TILESIZE = 60;
    private Player player;
    private KeyboardState prevState;
    private TileMaps tilemaps;
    private Texture2D rectangleTexture;
    private List<Rectangle> intersections;
    public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target) { //esto deberia estar en su propia clase, probablemente
        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
        int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;

        for (int x = 0; x <= widthInTiles; x++) {
            for (int y = 0; y <= heightInTiles; y++) {
                intersections.Add(new Rectangle(
                    (target.X + x * TILESIZE) / TILESIZE,
                    (target.Y + y * (TILESIZE - 1)) / TILESIZE,
                    TILESIZE,
                    TILESIZE
                ));
            }
        }
        return intersections;
    }
    public List<Rectangle> getIntersectingTilesVertical(Rectangle target) { //esto deberia estar en su propia clase, probablemente
        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
        int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;

        for (int x = 0; x <= widthInTiles; x++) {
            for (int y = 0; y <= heightInTiles; y++) {
                intersections.Add(new Rectangle(
                    (target.X + x * (TILESIZE - 1)) / TILESIZE,
                    (target.Y + y * TILESIZE) / TILESIZE,
                    TILESIZE,
                    TILESIZE
                ));
            }
        }
        return intersections;
    }
    public Game1() {
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

        Texture2D worldTexture = Content.Load<Texture2D>("worldTexture");
        tilemaps = new TileMaps(worldTexture, TILESIZE, 8);
        rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });

        player = new Player(
            Content.Load<Texture2D>("playerr"),
            new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE),
            new Rectangle(0, 0, TILESIZE, TILESIZE),
            Color.White
        );
        tilemaps.tilemap = tilemaps.LoadMap("Data/datas.csv"); // cambiar a ../../../Data/datas.csv si causa algun error
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){Exit();}

        //deberia pasar todo esto a una clase que maneje todas las colisiones, pero ni idea como manejar eso
        player.Update(gameTime, Keyboard.GetState(), prevState, viewport);

        player.Destrectangle.X += (int)player.velocity.X;
        intersections = getIntersectingTilesHorizontal(player.Destrectangle);

        foreach (var rect in intersections) {
            if (tilemaps.tilemap.TryGetValue(new Vector2(rect.X, rect.Y), out int _val)) {

                Rectangle collision = new(
                    rect.X * TILESIZE,
                    rect.Y * TILESIZE,
                    TILESIZE,
                    TILESIZE
                );

                if (player.velocity.X > 0.0f) {
                    player.Destrectangle.X = collision.Left - player.Destrectangle.Width;
                }
                else if (player.velocity.X < 0.0f) {
                    player.Destrectangle.X = collision.Right;
                }

            }

        }

        player.Destrectangle.Y += (int)player.velocity.Y;
        intersections = getIntersectingTilesVertical(player.Destrectangle);

        foreach (var rect in intersections) {

            if (tilemaps.tilemap.TryGetValue(new Vector2(rect.X, rect.Y), out int _val)) {

                Rectangle collision = new Rectangle(
                    rect.X * TILESIZE,
                    rect.Y * TILESIZE,
                    TILESIZE,
                    TILESIZE
                );

                if (player.velocity.Y > 0.0f) {
                    player.Destrectangle.Y = collision.Top - player.Destrectangle.Height;
                    player.velocity.Y = 1f;
                    player.onGround = true;
                }
                else if (player.velocity.Y < 0.0f) {
                    player.velocity.Y *= 0.1f;
                    player.Destrectangle.Y = collision.Bottom;
                }
            }
        }
        prevState = Keyboard.GetState();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(new Color(new Vector3(0.1176f, 0.1176f, 0.18039f)));
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        tilemaps.Draw(_spriteBatch);
        player.DrawSprite(_spriteBatch);

        // foreach (var rect in intersections) {
        //     DrawRectHollow(
        //         _spriteBatch,
        //         new Rectangle(
        //             rect.X * TILESIZE,
        //             rect.Y * TILESIZE,
        //             TILESIZE,
        //             TILESIZE
        //         ),
        //         4
        //     );
        // }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness) {
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Bottom - thickness,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.Right - thickness,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
    }
}
