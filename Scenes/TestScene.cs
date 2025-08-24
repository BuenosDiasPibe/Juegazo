using System;
using System.Collections.Generic;
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
            tilemap = new("Tiled/betterTest.tmx");
            var componentsOnEntity = new List<Component> {
                new CameraToEntityComponent(camera),
                new MoveHorizontalComponent(),
                new GodMovementVerticalComponent()
            };
            entities.Add(new Entity(playerTexture, new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE), new Rectangle(0,0, 50, 50), new Rectangle((int)(TILESIZE*0.3), (int)(TILESIZE*0.3), (int)(TILESIZE/0.3), (int)(TILESIZE/0.3)), componentsOnEntity,Color.White));
            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            camera.Zoom = 1;
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
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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