using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;

namespace Juegazo
{
    public interface IScene
    {
        public void LoadContent();
        public void Initialize(Game game);
        public void UnloadContent();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch);
    }
}