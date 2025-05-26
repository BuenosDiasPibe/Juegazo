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
        private ContentManager contentManager;

        private const int TILESIZE = 58;
        private List<Entity> entities;
        private List<Entity> entitiesDeleted;
        private List<WorldBlock> worldBlocks;
        private HitboxTilemaps collisionMap;
        private CollectableHitboxMap collectableHitboxMap;
        private GraphicsDevice graphicsDevice;
        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
        }
        public void LoadContent()
        {
            collisionMap = new HitboxTilemaps(contentManager.Load<Texture2D>("worldTexture"), TILESIZE, 8, 8);
            worldBlocks = new();
            collectableHitboxMap = new(contentManager.Load<Texture2D>("worldTexture"), TILESIZE, 8, 8, 0.5f);
            entities =
            [
                new Player(contentManager.Load<Texture2D>("playerr"),
                                        new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE),
                                        new Rectangle(TILESIZE, TILESIZE * 2, TILESIZE, TILESIZE), //donde aparece el jugador
                                        Color.White
                ),
            ];
            collisionMap.tilemap = collisionMap.LoadMap("Data/testLevel.csv"); // cambiar a ../../../Data/datas.csv si causa algun error
            foreach (var worldBlock in collisionMap.tilemap)
            {
                worldBlocks.Add(collisionMap.createWorld(worldBlock));
            }
            collectableHitboxMap.tilemap = collectableHitboxMap.LoadMap("Data/testLevel.csv");
            foreach (var item in collectableHitboxMap.tilemap)
            {
                entities.Add(new Collectable(contentManager.Load<Texture2D>("worldTexture"),
                                                collectableHitboxMap.BuildSourceRectangle(item),
                                                collectableHitboxMap.BuildDestinationRectangle(item),
                                                Color.White
                ));
            }
            entitiesDeleted = new();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            foreach (WorldBlock worldBlock in worldBlocks)
            {
                worldBlock.Update();
            }
            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case Player player:
                        var nonPlayers = entities.Where(e => e is not Player).ToList();
                        player.Update(gameTime, nonPlayers, worldBlocks, new());
                        entitiesDeleted = player.deleteCollectables;
                        break;
                    case Collectable collectable:
                        // Update collectable, passing all non-collectable entities
                        var nonCollectables = entities.Where(e => e is not Collectable).ToList();
                        collectable.Update(gameTime, nonCollectables, worldBlocks, new());
                        break;
                    default:
                        entity.Update(gameTime, entities, worldBlocks, new());
                        break;
                }
            }

            foreach (Entity entity in entitiesDeleted)
            {
                entities.Remove(entity);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            foreach (var worldBlock in worldBlocks)
            {
                worldBlock.DrawSprite(_spriteBatch);
            }
            foreach (var entity in entities)
            {
                entity.DrawSprite(_spriteBatch);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                new Debugger(graphicsDevice).drawhitboxEntities(_spriteBatch, entities, collisionMap, TILESIZE); // debugging for uuuh idk
            }
        }
    }
}