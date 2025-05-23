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
        private List<Collectable> collectables;
        private KeyboardState prevState;
        private HitboxTilemaps collisionMap;
        private CollectableHitboxMap collectableHitboxMap;
        private GraphicsDevice graphicsDevice;
        private int length;
        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
        }
        public void LoadContent()
        {
            collisionMap = new HitboxTilemaps(contentManager.Load<Texture2D>("worldTexture"), TILESIZE, 8, 8);
            collectableHitboxMap = new(contentManager.Load<Texture2D>("worldTexture"), TILESIZE, 8, 8, 0.5f);
            entities = new();
            collectables = new();
            entities.Add(new Player(contentManager.Load<Texture2D>("playerr"),
                                    new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE),
                                    new Rectangle(TILESIZE, TILESIZE * 2, TILESIZE, TILESIZE), //donde aparece el jugador
                                    Color.White
            ));
            collisionMap.tilemap = collisionMap.LoadMap("Data/testLevel.csv"); // cambiar a ../../../Data/datas.csv si causa algun error
            collectableHitboxMap.tilemap = collectableHitboxMap.LoadMap("Data/testLevel.csv");
            foreach (var item in collectableHitboxMap.tilemap)
            {
                collectables.Add(new Collectable(contentManager.Load<Texture2D>("worldTexture"),
                                                collectableHitboxMap.BuildSourceRectangle(item),
                                                collectableHitboxMap.BuildDestinationRectangle(item),
                                                Color.White
                ));
            }
            length = 0;
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            
            foreach (var entity in entities)
            {
                entity.Update(gameTime, Keyboard.GetState(), prevState, collectables);
                collectables = entity.collectables;
                collisionMap.Update(entity, TILESIZE);
            }

            prevState = Keyboard.GetState();
        }
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {

            collisionMap.Draw(_spriteBatch);
            foreach (var entity in entities)
            {
                entity.DrawSprite(_spriteBatch);
            }
            foreach(var col in collectables) {
                col.DrawSprite(_spriteBatch);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                new Debugger(graphicsDevice).drawhitboxEntities(_spriteBatch, entities, collisionMap, TILESIZE); // debugging for uuuh idk
            }
        }
    }
}