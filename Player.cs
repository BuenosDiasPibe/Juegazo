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
        public bool jumpPressed;
        public float verticalBoost;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();

            onGround = true;
            directionLeft = true;
            jumpPressed = false;
            prevState = new();
        }

        public override void Update(GameTime gameTime, List<Entity> entities, List<WorldBlock> worldBlocks, List<InteractiveBlock> interactiveBlocks)
        {
            CheckCollectables(entities);
            applyGravity();
            ManageVerticalMovement();
            HandleHorizontalMovement();
            ManageCollisions(worldBlocks);

            prevState = Keyboard.GetState();
        }

        private void ManageCollisions(List<WorldBlock> worldBlocks)
        {
            Destinationrectangle.X += (int)velocity.X;
            foreach (WorldBlock w in worldBlocks)
            {
                if (Destinationrectangle.Intersects(w.Destinationrectangle))
                {
                    w.blockType.horizontalActions(this, w.Destinationrectangle);
                }
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
        }

        private void ManageVerticalMovement()
        {
            // Jumping
            jumpPressed = (Keyboard.GetState().IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) ||
                               (Keyboard.GetState().IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));
            jumping(15);
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
        }

        public void jumping(float jumpAmmount)
        {
            if (onGround && jumpPressed)
            {
                velocity.Y -= jumpAmmount;
            }
        }

        private void HandleHorizontalMovement()
        {
            const float MOVEMENT_SPEED = 3f;
            const float MAX_SPEED = 60f;

            //inputs
            bool movingLeft = Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
            bool movingRight = Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            // Horizontal movement
            if (movingLeft)
            {
                if (!(velocity.X <= -10f))
                {
                    velocity.X += -MOVEMENT_SPEED;
                }
                directionLeft = true;
            }
            if (movingRight)
            {
                if (!(velocity.X >= 10f))
                {
                    velocity.X += MOVEMENT_SPEED;
                }
                directionLeft = false;
            }
            // Dash (sprint)
            if (Keyboard.GetState().IsKeyDown(Keys.B) && !prevState.IsKeyDown(Keys.B)) velocity.X = directionLeft ? -20 : 20;

            if (!movingLeft && !movingRight)
            {
                if (Math.Abs(velocity.X) > 1)
                {
                    velocity.X += velocity.X > 0 ? -1 : 1;
                }
                else
                {
                    velocity.X = 0;
                }
            }
            //max speed limit
            velocity.X = Math.Min(Math.Max(velocity.X, -MAX_SPEED), MAX_SPEED);
        }

        private void applyGravity()
        {
            // Apply gravity
            velocity.Y += 0.6f;
            velocity.Y = Math.Min(13, velocity.Y);
        }

        private void CheckCollectables(List<Entity> entities)
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
        }
    }
}