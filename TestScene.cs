using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gum.Wireframe;
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
        private readonly List<WorldBlock> worldBlocks = new();
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
            string CSVPath = "Data/goodOne.csv";
            worldTexture = contentManager.Load<Texture2D>("worldTexture");
            playerTexture = contentManager.Load<Texture2D>("playerr");
            collisionMap = new HitboxTilemaps(worldTexture, TILESIZE, 8, 8);
            collectableHitboxMap = new CollectableHitboxMap(worldTexture, TILESIZE, 8, 8, 0.5f);

            entities.Clear();
            worldBlocks.Clear();

            collisionMap.tilemap = collisionMap.LoadMap(CSVPath);
            foreach (var worldBlock in collisionMap.tilemap)
            {
                if (worldBlock.Value == 60)
                {
                    startPlayerposition = collisionMap.BuildDestinationRectangle(worldBlock);
                    entities.Add(new Player(playerTexture,
                                 new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE),
                                 startPlayerposition, camera,
                                 Color.White));
                    camera.Position = new Vector2(startPlayerposition.X, startPlayerposition.Y);
                }
                else
                {
                    worldBlocks.Add(collisionMap.createWorld(worldBlock));
                }
            }

            collectableHitboxMap.tilemap = collectableHitboxMap.LoadMap(CSVPath);
            foreach (var item in collectableHitboxMap.tilemap)
            {
                entities.Add(collectableHitboxMap.ReturnCollectable(item, worldTexture));
            }

            entitiesDeleted.Clear();
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
            foreach (var entity in entities.ToList())
            {
                switch (entity)
                {
                    case Player player:
                        var nonPlayers = entities.Where(e => e is not Player).ToList();
                        player.Update(gameTime, nonPlayers, worldBlocks, new());
                        entitiesDeleted.AddRange(player.deleteCollectables);
                        break;
                    case Collectable collectable:
                        var nonCollectables = entities.Where(e => e is not Collectable).ToList();
                        collectable.Update(gameTime, nonCollectables, worldBlocks, new());
                        break;
                    default:
                        entity.Update(gameTime, entities, worldBlocks, new());
                        break;
                }
            }
            
            foreach (var worldBlock in worldBlocks)
            {
                worldBlock.Update();
                if (!sceneChangeTriggered && worldBlock.blockType is CompleteBlock completeBlock)
                {
                    if (completeBlock.changeScene)
                    {
                        sceneChangeTriggered = true;
                        sceneManager.AddScene(new EndScene(sceneManager, contentManager, graphicsDevice, gum, camera));
                    }
                }
            }

            foreach (var entity in entitiesDeleted.Distinct())
            {
                entities.Remove(entity);
            }
            entitiesDeleted.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var worldBlock in worldBlocks)
            {
                worldBlock.DrawSprite(spriteBatch);
            }
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
                    spriteBatch.DrawString( font,
                                            $"Health {player.health}/{player.maxHealth}\nSprints: {player.dashCounter}\nJumpBoosts: {player.incrementJumps}",
                                            new Vector2(camera.Left, camera.Top), //OMFG IT WAS THAT EASLY IM GOING TO KMS
                                            Color.White);
                    break;
                }
            }
        }
    }
}