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
        public float sprint;
        public bool jumpPressed;
        public float pushBack;
        public float verticalBoost;
        public List<Collectable> collectables;

        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = true;
            sprint = 0;
            jumpPressed = false;
            pushBack = 0;
        }
        public abstract void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState, List<Collectable> collectables);
    }
}