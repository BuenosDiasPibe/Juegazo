using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public class Player : Entity
    {
        public bool directionLeft;
        public List<Entity> deleteCollectables;
        private KeyboardState prevState;

        public bool isDestroyed;
        public float sprint;
        public bool jumpPressed;
        public float pushBack;
        public float verticalBoost;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();

            onGround = true;
            sprint = 0;
            pushBack = 0;
            directionLeft = true;
            jumpPressed = false;
            prevState = new();
        }

        public override void Update(GameTime gameTime, List<Entity> entities, List<WorldBlock> worldBlocks, List<InteractiveBlock> interactiveBlocks)
        {
            deleteCollectables = new();
            foreach (Collectable collectable in entities)
            {
                if (collectable.Destinationrectangle.Intersects(Destinationrectangle))
                {
                    collectable.changeThings();
                    deleteCollectables.Add(collectable);
                }
            }
            // Apply gravity
            velocity.Y += 0.6f;
            velocity.Y = Math.Min(13, velocity.Y);

            // Horizontal movement
            velocity.X = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -10;
                directionLeft = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = 10;
                directionLeft = false;
            }
            // Dash (sprint)
            if (Keyboard.GetState().IsKeyDown(Keys.B) && !prevState.IsKeyDown(Keys.B)) sprint = directionLeft ? -30 : 30;

            // Apply sprint
            if (sprint != 0)
            {
                velocity.X += sprint;
                sprint += sprint > 0 ? -1 : 1;
            }

            // Apply 'pushBack'
            if (pushBack != 0)
            {
                velocity.X += pushBack;
                pushBack += pushBack > 0 ? -1 : 1;
            }
            Destinationrectangle.X += (int)velocity.X;
            foreach (WorldBlock w in worldBlocks)
            {
                if (Destinationrectangle.Intersects(w.Destinationrectangle))
                {
                    w.blockType.horizontalActions(this, w.Destinationrectangle);
                }
            }
            // Jumping
            jumpPressed = (Keyboard.GetState().IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) ||
                               (Keyboard.GetState().IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));
            if (onGround && jumpPressed)
            {
                velocity.Y = -12;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T) && !prevState.IsKeyDown(Keys.T))
            {
                velocity.Y = -12;
            }

            // Fast fall
            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) velocity.Y += 10;

            if (verticalBoost != 0)
            {
                velocity.Y += verticalBoost;
                if (velocity.Y < -12)
                {
                    verticalBoost = 0;
                }
                verticalBoost += verticalBoost > 0 ? -1 : 1;
            }
            Destinationrectangle.Y += (int)velocity.Y;
            onGround = false;
            foreach (WorldBlock w in worldBlocks)
            {
                if (Destinationrectangle.Intersects(w.Destinationrectangle))
                {
                    w.blockType.verticalActions(this, w.Destinationrectangle);
                }
            }
            prevState = Keyboard.GetState();
        }
    }
}