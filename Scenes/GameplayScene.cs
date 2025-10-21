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
    public class GameplayScene : IScene
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
        private string levelPath;
        private KeyboardState pastKey;
        private Debugger debugger;
        private bool enableDebugger;
        private bool changeScene = false;
        public bool cameraBoundries = true;
        private static string GetExecutingDir(string v)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDirectory, v);
        }

        public GameplayScene(ContentManager contentManager,
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
            changeScene = false;
            debugger = new(graphicsDevice);
            playerTexture = contentManager.Load<Texture2D>("second_player_sprite");
            if (levelPath == null)
            {
                levelPath = "Main.tmj";
            }

            List<ICustomTypeDefinition> typeDefinitions = new();
            tilemap = new(graphicsDevice, projectDirectory, levelPath, TILESIZE, typeDefinitions, gum);
            entities.AddRange(tilemap.entities);

            var componentsOnEntity = new List<Component> {
                new KeyboardInputComponent(),
                new CameraToEntitySimpleComponent(camera),
                new MoveVerticalComponent(),
                new MoveHorizontalComponent(),
                new ComplexGravityComponent(),
                new AnimationComponent(),
                new EntitiesInteractionsComponent(entities),
                new StateManagerComponent()
            };
            Rectangle playerPosition = new(TILESIZE * 12, TILESIZE * 2, TILESIZE, TILESIZE); // random position in the map, if it spawns there, something went wrong
            if (tilemap.EntityPositionerByName.TryGetValue("PlayerSpawner", out Vector2 position))
            {
                playerPosition = new((int)position.X, (int)position.Y, TILESIZE, TILESIZE);
                camera.Position = new(position.X, position.Y);
            }
            componentsOnEntity.Add(new CanDieComponent(new(playerPosition.X, playerPosition.Y)));
            Entity player = new Entity(playerTexture, new Rectangle(0, 0, playerTexture.Width, playerTexture.Height), playerPosition, componentsOnEntity, collider: 0.7f, Color.White);
            player.isPlayer = true;

            entities.Add(player);
            // get all the entities from Tiled

            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            camera.Zoom = tilemap.cameraZoom;
            cameraBoundries = tilemap.levelBoundries;
        }

        public void UnloadContent()
        {
            font = null;
            entities = new();
        }

        public void Update(GameTime gameTime)
        {
            int nextScene = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.R) && pastKey.IsKeyDown(Keys.R))
            {
                var playerEntity = entities.First(e => e.isPlayer);
                entities.Clear();
                entities.Add(playerEntity);
                entities.AddRange(tilemap.entities);
                var interactions = (EntitiesInteractionsComponent)playerEntity.GetComponent<EntitiesInteractionsComponent>();
                interactions.entities = entities;
                List<ICustomTypeDefinition> typeDefinitions = new();
                tilemap = new(graphicsDevice, projectDirectory, levelPath, TILESIZE, typeDefinitions, gum);
                entities.AddRange(tilemap.entities);
                camera.Zoom = tilemap.cameraZoom;
                cameraBoundries = tilemap.levelBoundries;
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
            // updating blocks
            var updatableBlocks = tilemap.dynamicBlocks.Values
                .Concat(tilemap.collisionLayer.Values)
                .Where(b => b != null && b.EnableUpdate);
            foreach (var block in updatableBlocks)
            {
                block.Update(gameTime);
            }
            var collisionBlocks = tilemap.collisionLayer.Values.Where(b => b.EnableCollisions);
            var dynamicBlocks = tilemap.dynamicBlocks.Values.Where(b => b.EnableCollisions);

            foreach (var entity in entities.ToList())
            {
                entity.Update(gameTime);
                entity.touchingWaterBlock = false;
                if (entity.componentList.Count == 0)
                {
                    entities.Remove(entity);
                    Console.WriteLine("deleted entity");
                }
                entity.Destinationrectangle.X += (int)entity.velocity.X;
                entity.Destinationrectangle.X += (int)entity.baseVelocity.X;

                foreach (Block block in collisionBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.horizontalActions(entity, block.collider);
                    nextScene = changeSScne(nextScene, block);
                }
                foreach (Block block in dynamicBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.horizontalActions(entity, block.collider);
                    
                    nextScene = changeSScne(nextScene, block);
                }

                entity.Destinationrectangle.Y += (int)entity.velocity.Y;
                entity.Destinationrectangle.Y += (int)entity.baseVelocity.Y;
                foreach (Block block in collisionBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.verticalActions(entity, block.collider);
                    nextScene = changeSScne(nextScene, block);
                }

                foreach (Block block in dynamicBlocks)
                {
                    if (block.collider.Intersects(entity.Destinationrectangle))
                        block.verticalActions(entity, block.collider);
                    nextScene = changeSScne(nextScene, block);
                }
                entity.UpdateColliderFromDest();
            }

            if (changeScene)
            {
                string testPath = Path.Combine(projectDirectory, "Level" + nextScene + ".tmj");
                if (!File.Exists(testPath))
                {
                    Console.WriteLine("----- Level does not exist, changing to Main.tmj -----");
                    nextScene = 0;
                }
                UnloadContent();
                if (nextScene == 0)
                {
                    levelPath = "Main.tmj";
                    LoadContent();
                    return;
                }
                levelPath = "Level" + nextScene + ".tmj";
                Console.WriteLine($"------------ Changing To {levelPath} ------------");
                LoadContent();
            }

            //killing entity if out of boundries
            foreach (var entity in entities)
            {
                if (entity.Destinationrectangle.X > tilemap.Width * TILESIZE || entity.Destinationrectangle.X < 0 || entity.Destinationrectangle.Y > tilemap.Height * TILESIZE || entity.Destinationrectangle.Y < 0)
                {
                    entity.health = 0;
                }
            }
            // Clamp camera position within map bounds, copilot made this 
            // i dont know how math works im really sorry :(
            if (cameraBoundries)
            {
                camera.X = MathHelper.Clamp(camera.X, 
                    camera.ViewPortRectangle.Width / 2, 
                    tilemap.Width * TILESIZE - camera.ViewPortRectangle.Width / 2);
                camera.Y = MathHelper.Clamp(camera.Y,
                    camera.ViewPortRectangle.Height / 2,
                    tilemap.Height * TILESIZE - camera.ViewPortRectangle.Height / 2);
            }

            pastKey = Keyboard.GetState();
        }

        private int changeSScne(int nextScene, Block block)
        {
            if (block is CompleteBlock papu && papu.changeScene && !changeScene)
            {
                changeScene = true;
                nextScene = papu.nextSceneID;
            }

            return nextScene;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tilemap.Draw(gameTime, spriteBatch, camera, entities);
            foreach (var entity in entities)
            {
                // if there's no Entity Spawner in the level, show the entities anyway
                if (tilemap.EntityPositionerByName.Count == 0)
                    entity.Draw(gameTime, spriteBatch);

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
                else debugger.DrawRectHollow(spriteBatch, p.collider, 2, new Color(50, 50, 50, 100));
            }
            debugger.DrawRectHollow(spriteBatch, camera.ViewPortRectangle, 2, Color.White);
        }

        public void Initialize(Game game)
        { }
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                if (entity.hasComponent(typeof(NPCComponent)) && entity.GetComponent(typeof(NPCComponent)) is NPCComponent comp)
                {
                    comp.DrawUI(gameTime, spriteBatch);
                    continue;
                }
                if(entity.isPlayer)
                    spriteBatch.DrawString(font,
                        $"Level: {Path.GetFileNameWithoutExtension(levelPath)}\nFPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}\nposition: X:{entity.Destinationrectangle.X} Y:{entity.Destinationrectangle.Y}\nvelocity: {entity.velocity}\nbaseVelocity: {entity.baseVelocity}\nhealth: {entity.health}\nstate: {entity.entityState}",
                        new Vector2(camera.Left, camera.Top),
                            Color.White);
            }
            spriteBatch.DrawString(font, camera.ToString(), new(camera.Left, camera.Top + 300), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
        }
    }
}