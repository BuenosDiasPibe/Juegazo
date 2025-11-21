using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarinMol.Scenes;
public interface IScene
{
    public void LoadContent();
    //public void Initialize(Game game);
    public void UnloadContent();
    public void Update(GameTime gameTime);
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch);
}
