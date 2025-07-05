using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public abstract class Entity : Sprite
    {
        //TODO: rewrite Entity class, or remake it to later use a class for enemies and player
        public Vector2 velocity;
        public bool onGround;
        public bool horizontalBlockMovementAction;

        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = false;
            horizontalBlockMovementAction = false;
        }
        public abstract void Update(GameTime gameTime,
        List<Entity> entities, List<WorldBlock> worldBlocks,List<InteractiveBlock> interactiveBlocks);
    }
}