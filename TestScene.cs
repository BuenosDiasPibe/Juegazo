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
        private KeyboardState prevState;
        private HitboxTilemaps collisionMap;
        public TestScene(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }
        public void LoadContent()
        {
            collisionMap = new HitboxTilemaps(contentManager
            .Load<Texture2D>("worldTexture"), TILESIZE, 8);
            entities = new();
            entities.Add(new Player(contentManager
            .Load<Texture2D>("playerr"),
                                    new Rectangle(TILESIZE, TILESIZE, TILESIZE, TILESIZE),
                                    new Rectangle(TILESIZE, TILESIZE * 2, TILESIZE, TILESIZE), //donde aparece el jugador
                                    Color.White
            ));
            collisionMap.tilemap = collisionMap.LoadMap("Data/testLevel.csv"); // cambiar a ../../../Data/datas.csv si causa algun error
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime, Keyboard.GetState(), prevState);
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
            //new Debugger(GraphicsDevice).drawhitboxEntities(_spriteBatch, entities, collisionMap, TILESIZE); // debugging for uuuh idk
        }
    }
}