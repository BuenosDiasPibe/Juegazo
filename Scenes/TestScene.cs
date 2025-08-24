using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gum.Wireframe;
using Juegazo.Components;
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
        private readonly List<Entity> entitiesDeleted = new();
        private HitboxTilemaps collisionMap;
        private CollectableHitboxMap collectableHitboxMap;
        private Rectangle startPlayerposition;
        GumService gum;
        private SpriteFont font;
        Camera camera;
        Texture2D worldTexture;
        Texture2D playerTexture;
        bool sceneChangeTriggered = false;

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
            Console.WriteLine("damn");
            worldTexture = contentManager.Load<Texture2D>("worldTexture");
            playerTexture = contentManager.Load<Texture2D>("playerr");

            entities.Clear();

            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            camera.Zoom = 2;
            sceneChangeTriggered = false;
        }

        public void UnloadContent()
        {
            collisionMap = null;
            collectableHitboxMap = null;
            font = null;
            worldTexture.Dispose();
            playerTexture.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                entity.DrawSprite(spriteBatch);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                new Debugger(graphicsDevice).drawhitboxEntities(spriteBatch, entities, collisionMap, TILESIZE);
            }
        }

        public void Initialize(Game game)
        {
        }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                if (entity is Player player)
                {
                    spriteBatch.DrawString(font,
                                            $"Health {player.health}/{player.maxHealth}\nSprints: {player.dashCounter}\nJumpBoosts: {player.incrementJumps}",
                                            new Vector2(camera.Left, camera.Top), //OMFG IT WAS THAT EASLY IM GOING TO KMS
                                            Color.White);
                    break;
                }
            }
            spriteBatch.DrawString(font, 
                $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}", 
                new Vector2(camera.Left, camera.Top + 60), 
                Color.White);
        }
    }
}