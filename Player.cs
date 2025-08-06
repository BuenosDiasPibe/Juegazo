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
        private int numJumps;
        public int jumpCounter;
        public int incrementJumps;
        public int numDash;
        public Vector2 initialPosition;
        public int dashCounter;
        public Camera camera;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Color color, Camera camera) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();

            onGround = true;
            directionLeft = true;
            jumpPressed = false;

            prevState = new();
            numJumps = 1;
            jumpCounter = 0;
            incrementJumps = 0;

            numDash = 0;
            dashCounter = 0;

            initialPosition = new Vector2(Destrectangle.X, Destrectangle.Y);
            this.camera = camera;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, List<WorldBlock> worldBlocks, List<InteractiveBlock> interactiveBlocks)
        {
            CheckCollectables(entities);
            ApplyGravity();
            ManageVerticalMovement();
            HandleHorizontalMovement();
            ManageCollisions(worldBlocks);
            if (health < 0)
            {
                HandleDeath();
            }

            prevState = Keyboard.GetState();
            camera.Position = new Vector2(Destinationrectangle.X, Destinationrectangle.Y);
        }
        public void HandleDeath()
        {
            velocity = new Vector2();
            Destinationrectangle.X = (int)initialPosition.X;
            Destinationrectangle.Y = (int)initialPosition.Y;
            health = maxHealth;
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

            if (!onGround && jumpCounter == 0)
            {
                jumpCounter++;
            }
        }

        private void ManageVerticalMovement()
        {
            // Jumping
            jumpPressed = ( Keyboard.GetState().IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) || (Keyboard.GetState().IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));

            Jumping(11);

            if (Keyboard.GetState().IsKeyDown(Keys.T) && !prevState.IsKeyDown(Keys.T)) velocity.Y = -7;

            // Fast fall
            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) velocity.Y += 10;

            if (verticalBoost != 0)
            {
                velocity.Y += verticalBoost;
                if (velocity.Y < -10)
                {
                    verticalBoost = 0;
                }
                verticalBoost += verticalBoost > 0 ? -1 : 1;
            }
        }

        public void Jumping(float jumpAmmount)
        {
            if (incrementJumps > 0 && jumpPressed)
            {
                velocity.Y -= Math.Min(Math.Max(jumpAmmount, -11), 11);
                incrementJumps--;   
            }
            else if (onGround && jumpPressed && jumpCounter < numJumps)
            {
                velocity.Y -= jumpAmmount;
                jumpCounter++;
            }
        }

        private void HandleHorizontalMovement()
        {
            const float MOVEMENT_SPEED = 3f;
            const float MAX_SPEED = 30f;

            //inputs
            bool movingLeft = Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
            bool movingRight = Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            // Horizontal movement
            if (movingLeft)
            {
                if (!(velocity.X <= -5f))
                {
                    velocity.X += -MOVEMENT_SPEED;
                }
                directionLeft = true;
            }
            if (movingRight)
            {
                if (!(velocity.X >= 5f))
                {
                    velocity.X += MOVEMENT_SPEED;
                }
                directionLeft = false;
            }
            // Dash (sprint)
            if (dashCounter > 0 && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !prevState.IsKeyDown(Keys.LeftShift)) {
                velocity.X += directionLeft ? -7 : 7;
                dashCounter--;
            }

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

        private void ApplyGravity()
        {
            // Apply gravity
            velocity.Y += 0.6f;
            velocity.Y = Math.Min(10, velocity.Y);
        }

        private void CheckCollectables(List<Entity> entities)
        {
            deleteCollectables = [];
            foreach (Entity entity in entities)
            {
                if (entity is Collectable collectable && collectable.Destinationrectangle.Intersects(Destinationrectangle))
                {
                    collectable.changeThings(this);
                    deleteCollectables.Add(collectable);
                }
            }
        }
    }
}