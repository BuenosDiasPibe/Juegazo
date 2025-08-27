using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gum.Wireframe;
using Juegazo.Components;
using Juegazo.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using RenderingLibrary.Graphics;

namespace Juegazo
{
    public class TestScene : IScene
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;
        private SceneManager sceneManager;

        private const int TILESIZE = 32;
        private List<Entity> entities = new();
        GumService gum;
        private SpriteFont font;
        Camera camera;
        Texture2D playerTexture;
        private TiledMap tilemap;
        private Block block; //testing
        private string projectDirectory = GetExecutingDir("../../../Tiled");

        private static string GetExecutingDir(string v)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDirectory, v);
        }

        public TestScene(ContentManager contentManager,
        GraphicsDevice graphicsDevice,
        GumService gum,
        SceneManager sceneManager,
        Camera camera)
        {
            this.contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            this.graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            this.sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
            this.gum = gum;
            this.camera = camera;
        }

        public void LoadContent()
        {
            playerTexture = contentManager.Load<Texture2D>("playerr");
            tilemap = new(graphicsDevice, projectDirectory, "betterTest.tmx", TILESIZE);
            var componentsOnEntity = new List<Component> {
                new CameraToEntityComponent(camera),
                new MoveVerticalComponent(),
                new MoveHorizontalComponent(),
                new ComplexGravityComponent(),
            };
            entities.Add(new Entity(playerTexture, new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE), new Rectangle(TILESIZE*2,TILESIZE*2, TILESIZE, TILESIZE), componentsOnEntity,Color.White));
            //entities.Add(new Player(playerTexture, new Rectangle(TILESIZE,TILESIZE,TILESIZE,TILESIZE), new Rectangle(TILESIZE*2, TILESIZE*2, TILESIZE, TILESIZE), camera, Color.White, componentsOnEntity));
            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            camera.Zoom = 1.5f;
        }

        public void UnloadContent()
        {
            font = null;
            playerTexture.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
                entity.Destinationrectangle.X += (int)entity.velocity.X;
                foreach (Block block in tilemap.collisionLayer.Values)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                    {
                        this.block = block;
                        block.horizontalActions(entity, block.collider);
                    }
                }

                entity.Destinationrectangle.Y += (int)entity.velocity.Y;
                foreach(Block block in tilemap.collisionLayer.Values)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                    {
                        block.verticalActions(entity, block.collider);
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tilemap.Draw(spriteBatch);
            foreach (var entity in entities)
            {
                entity.DrawSprite(spriteBatch);
                new Debugger(graphicsDevice).DrawRectHollow(spriteBatch, entity.collider, 2);
                new Debugger(graphicsDevice).DrawRectHollow(spriteBatch, entity.Destinationrectangle, 2);
            }
        }

        public void Initialize(Game game)
        { }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities) {
            spriteBatch.DrawString(font, 
                $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}\nvelocity: {entity.velocity}\nColliding With block: {block}", 
                new Vector2(camera.Left, camera.Top + 60),
                    Color.White);        }
        }
    }
}