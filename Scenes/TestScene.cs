using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotTiled;
using Gum.Wireframe;
using Juegazo.Components;
using Juegazo.Map;
using Juegazo.Map.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using RenderingLibrary.Graphics;
using Color = Microsoft.Xna.Framework.Color;

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
        private KeyboardState pastKey;
        private Debugger debugger;
        private bool enableDebugger;

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
            debugger = new(graphicsDevice);
            playerTexture = contentManager.Load<Texture2D>("playerr");

            List<ICustomTypeDefinition> typeDefinitions = new();
            tilemap = new(graphicsDevice, projectDirectory, "betterTest.tmx", TILESIZE, typeDefinitions);
            var componentsOnEntity = new List<Component> {
                new CameraToEntityComponent(camera),
                new MoveVerticalComponent(),
                new MoveHorizontalComponent(),
                new ComplexGravityComponent(),
            };
            Rectangle playerPosition = new(TILESIZE * 12, TILESIZE * 2, TILESIZE, TILESIZE); // random position in the map, if it spawns there, something went wrong
            if (tilemap.EntityPositionerByName.TryGetValue("PlayerSpawner", out Vector2 position))
            {
                playerPosition = new((int)position.X, (int)position.Y, TILESIZE, TILESIZE);
            }
            componentsOnEntity.Add(new CanDieComponent(new(playerPosition.X, playerPosition.Y)));
            entities.Add(new Entity(playerTexture, new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE), playerPosition, componentsOnEntity, collider: 0.7f, Color.White));
            //entities.Add(new Player(playerTexture, new Rectangle(TILESIZE,TILESIZE,TILESIZE,TILESIZE), new Rectangle(TILESIZE*2, TILESIZE*2, TILESIZE, TILESIZE), camera, Color.White, componentsOnEntity));
            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            camera.Zoom = 1.5f;
        }

        public void UnloadContent()
        {
            font = null;
            entities = new();
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && pastKey.IsKeyDown(Keys.R))
            {
                List<ICustomTypeDefinition> typeDefinitions = new();
                tilemap = new(graphicsDevice, projectDirectory, "betterTest.tmx", TILESIZE, typeDefinitions);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M) && pastKey.IsKeyDown(Keys.M))
            {
                UnloadContent();
                sceneManager.RemoveScene();
            }
            //TODO: why am i doing this? :skull:
            if (Keyboard.GetState().IsKeyDown(Keys.F3) && pastKey.IsKeyUp(Keys.F3))
            {
                enableDebugger = !enableDebugger;
            }
            foreach (var block in tilemap.dynamicBlocks.Values)
            {
                block.Update(gameTime);
            }
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
                entity.Destinationrectangle.X += (int)entity.velocity.X;

                var collisionBlocks = tilemap.collisionLayer.Values.Where(b => b.EnableCollisions);
                var dynamicBlocks = tilemap.dynamicBlocks.Values.Where(b => b.EnableCollisions);
                foreach (Block block in collisionBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.horizontalActions(entity, block.collider);
                    if (block is CompleteBlock papu && papu.changeScene == true)
                        sceneManager.AddScene(new EndScene(sceneManager, contentManager, graphicsDevice, gum, camera));
                }
                foreach (Block block in dynamicBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.horizontalActions(entity, block.collider);
                    if (block is CompleteBlock papu && papu.changeScene == true)
                        sceneManager.AddScene(new EndScene(sceneManager, contentManager, graphicsDevice, gum, camera));
                }

                entity.Destinationrectangle.Y += (int)entity.velocity.Y;
                foreach (Block block in collisionBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.verticalActions(entity, block.collider);
                    if (block is CompleteBlock papu && papu.changeScene == true)
                        sceneManager.AddScene(new EndScene(sceneManager, contentManager, graphicsDevice, gum, camera));
                }

                foreach (Block block in dynamicBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.horizontalActions(entity, block.collider);
                    if (block is CompleteBlock papu && papu.changeScene)
                        sceneManager.AddScene(new EndScene(sceneManager, contentManager, graphicsDevice, gum, camera));
                }
                foreach (Block block in tilemap.dynamicBlocks.Values)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.verticalActions(entity, block.collider);
                    if (block is CompleteBlock papu && papu.changeScene)
                        sceneManager.AddScene(new EndScene(sceneManager, contentManager, graphicsDevice, gum, camera));
                }
                entity.UpdateColliderFromDest();
            }
            pastKey = Keyboard.GetState();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tilemap.Draw(spriteBatch);
            foreach (var entity in entities)
            {
                entity.DrawSprite(spriteBatch);
                if (enableDebugger)
                {
                    debugger.DrawRectHollow(spriteBatch, entity.collider, 2, Color.Red);
                    debugger.DrawRectHollow(spriteBatch, entity.Destinationrectangle, 2, Color.Blue);
                }
            }
            if (!enableDebugger) return;
            foreach (var t in tilemap.collisionLayer.Values)
            {
                if (t.EnableCollisions) debugger.DrawRectHollow(spriteBatch, t.collider, 2, Color.Green);
                else debugger.DrawRectHollow(spriteBatch, t.collider, 2, new Color(25, 25, 25, 100));
            }
            foreach (var p in tilemap.dynamicBlocks.Values)
            {

                if (p.EnableCollisions) debugger.DrawRectHollow(spriteBatch, p.collider, 4, Color.Brown);
                else debugger.DrawRectHollow(spriteBatch, p.collider, 2, new Color(50,50,50,100));
            }
        }

        public void Initialize(Game game)
        { }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                spriteBatch.DrawString(font,
                    $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}\nposition: X:{entity.Destinationrectangle.X} Y:{entity.Destinationrectangle.Y}\nvelocity: {entity.velocity}",
                    new Vector2(camera.Left, camera.Top),
                        Color.White);
            }
            spriteBatch.DrawString(font,
                                    $"exit game: {"Escape"}\nReload: {"R"}\nMain Menu: {"M"}", //TODO: add the keys to variables so i dont need to change this every time
                                    new Vector2(camera.Right - 200, camera.Top),
                                    Color.White);
        }
    }
}