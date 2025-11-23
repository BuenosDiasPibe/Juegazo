using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotTiled;
using Juegazo.EntityComponents;
using Juegazo.Map;
using Juegazo.Map.Blocks;
using MarinMol;
using MarinMol.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Color = Microsoft.Xna.Framework.Color;
using Debugger = MarinMol.Debugger;

namespace Juegazo
{
    public class GameplayScene(ContentManager contentManager,
    GraphicsDevice graphicsDevice,
    SceneManager sceneManager) : IScene
    {
        private readonly ContentManager contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
        private readonly GraphicsDevice graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        private readonly SceneManager sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
        private const int TILESIZE = 32;
        private List<Entity> entities = [];
        private SpriteFont font;
        Texture2D playerTexture;
        private TiledMap tilemap;
        private readonly string projectDirectory = GetExecutingDir("Tiled");
        private KeyboardState pastKey;
        private bool changeScene = false;
        public bool cameraBoundries = true;
        public AudioImoporter a;
        public double fps;
        private readonly Camera camera = Camera.Instance;

        public string levelStart = "";
        public string levelPath = "";
        private PauseScene pause;
        public RenderTarget2D lastScreen;
        private static string GetExecutingDir(string v) // TODO: create a Utilities singleton
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dirInfo = new DirectoryInfo(baseDirectory);
            for (int i = 0; i < 3 && dirInfo.Parent != null; i++)
            {
                dirInfo = dirInfo.Parent;
            }
            baseDirectory = dirInfo.FullName;
            return Path.Combine(baseDirectory, v);
        }

        public void LoadContent()
        {
            GumService.Default.Root.Children.Clear();
            pause = new(sceneManager, this);
            changeScene = false;
            lastScreen = new(graphicsDevice, sceneManager.viewport.Width, sceneManager.viewport.Height);
            playerTexture = contentManager.Load<Texture2D>("second_player_sprite");
            if(levelPath.Equals(""))
            {
              levelPath = "Main.tmj";
            }

            List<ICustomTypeDefinition> typeDefinitions = [];
            tilemap = new(graphicsDevice, projectDirectory, levelPath, TILESIZE, typeDefinitions);
            if(tilemap.loadAudio)
            {
                var sw = new Stopwatch();
                sw.Start(); a = new("Content/Sounds/Sfx"); sw.Stop();
                Console.WriteLine($"soundsLoad: {sw.ElapsedMilliseconds}ms");
                a.allSFXToBlocks([.. tilemap.collisionLayer.Values]);
            }
            var playableEntities = tilemap.entities
                .Where(e => e.isPlayable);
            foreach (var t in playableEntities)
            {
                var componentsOnEntity = new List<Component> {
                    new KeyboardInputComponent(),
                    new MoveVerticalComponent(),
                    new MoveHorizontalComponent(),
                    new ComplexGravityComponent(),
                    new EntitiesInteractionsComponent(entities),
                    new StateManagerComponent()
                };
                t.AddComponents(componentsOnEntity);
                t.AddComponent(typeof(CanDieComponent), new CanDieComponent(new(t.Destinationrectangle.X, t.Destinationrectangle.Y)));
                if (t.isPlayer)
                {
                    t.AddComponent(typeof(CameraToEntityComponent), new CameraToEntityComponent());
                    camera.Position = new(t.collider.X, t.collider.Y);
                    t.texture = playerTexture;
                    t.AddComponent(typeof(AnimationComponent), new AnimationComponent());
                }
            }
            entities.AddRange(tilemap.entities);

            font = contentManager.Load<SpriteFont>("sheesh");
            camera.Origin = new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);
            if (tilemap.cameraZoom != 0)
            {
                camera.Zoom = tilemap.cameraZoom;
            }
            else
            {
                camera.Zoom = camera.Viewport.Height / tilemap.Height; //put the camera with height of levelHeight
            }
            cameraBoundries = tilemap.levelBoundries;
        }

        public void UnloadContent()
        {
          font = null;
          entities = [];
        }
        bool wasPressed = true;

        public void Update(GameTime gameTime)
        {
          if(!wasPressed && Keyboard.GetState().IsKeyDown(Keys.Escape))
          {
            wasPressed = true;
            sceneManager.AddScene(pause);
            return;
          }
          if(Keyboard.GetState().IsKeyUp(Keys.Escape)){wasPressed = false;}

          int nextScene = 0;

          if (Keyboard.GetState().IsKeyDown(Keys.R) && pastKey.IsKeyDown(Keys.R))
          {
              Console.WriteLine("-- reloading --");
              UnloadContent();
              LoadContent();
          }
          if (Keyboard.GetState().IsKeyDown(Keys.M) && pastKey.IsKeyDown(Keys.M))
          {
              UnloadContent();
              sceneManager.RemoveAndLoadLastScene();
          }
          if (Keyboard.GetState().IsKeyDown(Keys.F3) && pastKey.IsKeyUp(Keys.F3))
          {
            Debugger.Instance.drawDebug = !Debugger.Instance.drawDebug;
          }
          // updating blocks
          var updatableBlocks = tilemap.collisionLayer.Values
              .Concat(tilemap.collisionLayer.Values)
              .Where(b => b != null && b.EnableUpdate);
          foreach (var block in updatableBlocks)
          {
              block.Update(gameTime);
          }

          var collisionBlocks = tilemap.collisionLayer.Values.Where(b => b.EnableCollisions);

          foreach (var entity in entities.ToList())
          {
              entity.Update(gameTime);
              entity.touchingWaterBlock = false;
              FACES entityDirection = entity.direction;
              if (entity.componentList.Count == 0)
              {
                  entities.Remove(entity);
                  Console.WriteLine("deleted entity");
              }

              int horizDelta = 0;
              if (entityDirection == FACES.LEFT)
                  horizDelta -= (int)entity.velocity.Y + (int)entity.baseVelocity.Y;
              else if (entityDirection == FACES.RIGHT)
                  horizDelta += (int)entity.velocity.Y + (int)entity.baseVelocity.Y;
              else 
                  horizDelta += (int)entity.velocity.X + (int)entity.baseVelocity.X;

              if(entityDirection == FACES.TOP)
                  entity.Destinationrectangle.X += horizDelta;
              else
                  entity.Destinationrectangle.X += horizDelta;

              foreach (Block block in collisionBlocks)
              {
                  if (block.collider.Intersects(entity.Destinationrectangle))
                      block.horizontalActions(entity, block.collider);
                  nextScene = ChangeSScne(nextScene, block);
              }

              int vertDelta = 0;
              if (entityDirection == FACES.BOTTOM)
                  vertDelta += (int)entity.velocity.Y + (int)entity.baseVelocity.Y;
              else if (entityDirection == FACES.TOP)
                  vertDelta -= (int)entity.velocity.Y + (int)entity.baseVelocity.Y;
              else
                  vertDelta += (int)entity.velocity.X + (int)entity.baseVelocity.X;
              if(entityDirection == FACES.RIGHT)
                  entity.Destinationrectangle.Y -= vertDelta;
              else
                  entity.Destinationrectangle.Y += vertDelta;

              foreach (Block block in collisionBlocks)
              {
                  if (block.collider.Intersects(entity.Destinationrectangle))
                      block.verticalActions(entity, block.collider);
                  nextScene = ChangeSScne(nextScene, block);
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
              if (entity.Destinationrectangle.X > tilemap.MapWidth * TILESIZE || entity.Destinationrectangle.X < 0 || entity.Destinationrectangle.Y > tilemap.MapHeight * TILESIZE || entity.Destinationrectangle.Y < 0)
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
                  tilemap.MapWidth * TILESIZE - camera.ViewPortRectangle.Width / 2);
              camera.Y = MathHelper.Clamp(camera.Y,
                  camera.ViewPortRectangle.Height / 2,
                  tilemap.MapHeight * TILESIZE - camera.ViewPortRectangle.Height / 2);
          }

          pastKey = Keyboard.GetState();
          fps = Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds);
        }

        private int ChangeSScne(int nextScene, Block block)
        {
            if (block is CompleteLevelBlock papu && papu.changeScene && !changeScene)
            {
                changeScene = true;
                nextScene = papu.nextScene;
            }
            return nextScene;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tilemap.Draw(gameTime, spriteBatch, entities);
            foreach (var entity in entities)
            {
                if (tilemap.EntityPositionerByName.Count == 0)
                    entity.Draw(gameTime, spriteBatch);

                Debugger.Instance.DrawRectHollow(spriteBatch, entity.collider, 2, Color.Red);
                Debugger.Instance.DrawRectHollow(spriteBatch, entity.Destinationrectangle, 2, Color.Blue);
            }
            //TODO: this can be sent to a list of actions to do when called Debugger
            foreach (var t in tilemap.collisionLayer.Values)
            {
                if (t.EnableCollisions) Debugger.Instance.DrawRectHollow(spriteBatch, t.collider, 2, Color.Green);
                else Debugger.Instance.DrawRectHollow(spriteBatch, t.collider, 2, new Color(25, 25, 25, 100));
            }
            Debugger.Instance.DrawRectHollow(spriteBatch, Camera.Instance.ViewPortRectangle, 2, Color.White);
        }

        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
          foreach (var entity in entities)
          {
              if (entity.hasComponent<NPCComponent>() && entity.GetComponent<NPCComponent>() is NPCComponent comp)
              {
                  comp.DrawUI(gameTime, spriteBatch);
                  continue;
              }
              if(entity.isPlayer)
              {
                string level = $"Level: {Path.GetFileNameWithoutExtension(levelPath)}\nFPS: {fps}\nposition: (X:{entity.Destinationrectangle.X} Y:{entity.Destinationrectangle.Y})\nvelocity: {entity.velocity}\nbaseVelocity: {entity.baseVelocity}\nhealth: {entity.health}\nstate: {entity.entityState}\ngravity: {entity.direction}";
                spriteBatch.DrawString(font,
                    level,
                    new Vector2(camera.Left, camera.Top),
                        Color.White);
              }
          }
          string showCamera = camera.ToString();
          var b = font.MeasureString(showCamera);
          spriteBatch.DrawString(font, showCamera, new(camera.Right-b.X*0.7f, camera.Top), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
        }
    }
}
