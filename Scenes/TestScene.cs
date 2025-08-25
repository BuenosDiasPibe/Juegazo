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
                new MoveHorizontalComponent(),
                new GodMovementVerticalComponent()
            };
            entities.Add(new Entity(playerTexture, new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE), new Rectangle(0,0, TILESIZE, TILESIZE), new Rectangle((int)(TILESIZE*0.3), (int)(TILESIZE*0.3), (int)(TILESIZE/0.3), (int)(TILESIZE/0.3)), componentsOnEntity,Color.White));
            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            camera.Zoom = 3;
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
                entity.collider.X += (int)entity.velocity.X;
                entity.collider.Y += (int)entity.velocity.Y;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tilemap.Draw(spriteBatch);
            foreach (var entity in entities)
            {
                entity.DrawSprite(spriteBatch);
            }
        }

        public void Initialize(Game game)
        {
        }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, 
                $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}", 
                new Vector2(camera.Left, camera.Top + 60), 
                Color.White);
        }
    }
}