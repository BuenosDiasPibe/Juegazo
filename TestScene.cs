using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public class TestScene : IScene
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;

        private const int TILESIZE = 58;
        private List<Entity> entities = new();
        private readonly List<Entity> entitiesDeleted = new();
        private readonly List<WorldBlock> worldBlocks = new();
        private HitboxTilemaps collisionMap;
        private CollectableHitboxMap collectableHitboxMap;
        private Rectangle startPlayerposition;

        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            this.graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        }

        public void LoadContent()
        {
            var worldTexture = contentManager.Load<Texture2D>("worldTexture");
            collisionMap = new HitboxTilemaps(worldTexture, TILESIZE, 8, 8);
            collectableHitboxMap = new CollectableHitboxMap(worldTexture, TILESIZE, 8, 8, 0.5f);

            entities.Clear();
            worldBlocks.Clear();

            collisionMap.tilemap = collisionMap.LoadMap("Data/testLevel.csv");
            foreach (var worldBlock in collisionMap.tilemap)
            {
                if (worldBlock.Value == 60)
                {
                    startPlayerposition = collisionMap.BuildDestinationRectangle(worldBlock);
                }
                else
                {
                    worldBlocks.Add(collisionMap.createWorld(worldBlock));
                }
            }
            var player = new Player(
                contentManager.Load<Texture2D>("playerr"),
                new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE),
                startPlayerposition,
                Color.White
            );
            entities.Add(player);


            collectableHitboxMap.tilemap = collectableHitboxMap.LoadMap("Data/testLevel.csv");
            foreach (var item in collectableHitboxMap.tilemap)
            {
                entities.Add(collectableHitboxMap.ReturnCollectable(item, worldTexture));
                Console.WriteLine("yeoieee");
            }

            entitiesDeleted.Clear();
        }

        public void UnloadContent()
        {
            // Implement resource cleanup if needed
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
    }
}