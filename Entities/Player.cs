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
        public List<Entity> deleteCollectables = [];
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

        private int lookAhead;
        int cameraHorizontal = 0;
        public int cameraVertical = 0;
        public Camera camera;
        public bool zoomInCamera;
        public bool zoomOutCamera;
        public GameTime gameTime;
        public bool hasJumpedWall = false;

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Camera camra, Color color, float collider) : base(texture, sourceRectangle, Destrectangle, collider, color)
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
            lookAhead = 0;
            camera = camra;

            initialPosition = new Vector2(Destrectangle.X, Destrectangle.Y);
        }

        public Player(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Camera camra, Color color, List<Component> components, float collider) : base(texture, sourceRectangle, Destrectangle, collider, color)
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
            lookAhead = 0;
            camera = camra;

            initialPosition = new Vector2(Destrectangle.X, Destrectangle.Y);
            foreach (var component in components)
            {
                this.AddComponent(component.GetType(), component);
            }
        }
        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            ApplyGravity();
            ManageVerticalMovement();
            HandleHorizontalMovement();
            cameraManager(gameTime);
            if (health <= 0)
            {
                HandleDeath();
            }

            prevState = Keyboard.GetState();
        }

        private void cameraManager(GameTime gameTime)
        {
            cameraHorizontal = (int)MathHelper.Lerp(cameraHorizontal, Destinationrectangle.X + lookAhead + Destinationrectangle.Width / 2, 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);

            if (onGround || incrementJumps > 0 || hasJumpedWall)
            {
                cameraVertical = Destinationrectangle.Y + Destinationrectangle.Height / 2;
            }

            Vector2 targetPosition = new Vector2(cameraHorizontal, cameraVertical);
            camera.Position = Vector2.Lerp(camera.Position, targetPosition, 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);
        }

        private void ZoomInCamera()
        {
            camera.Zoom = MathHelper.Lerp(camera.Zoom, 3f, 0.1f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);
            if (Math.Abs(camera.Zoom - 3f) < 0.01f)
            {
                zoomInCamera = false;
            }
        }
        private void ZoomOutCamera()
        {
            camera.Zoom = MathHelper.Lerp(camera.Zoom, 1f, 0.1f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);
            if (Math.Abs(camera.Zoom - 1f) < 0.01f)
            {
                zoomOutCamera = false;
            }
        }

        public void HandleDeath()
        {
            velocity = new Vector2();
            Destinationrectangle.X = (int)initialPosition.X;
            Destinationrectangle.Y = (int)initialPosition.Y;
            health = maxHealth;
            camera.Position = new Vector2(Destinationrectangle.X + lookAhead - Destinationrectangle.Width / 2, Destinationrectangle.Y + Destinationrectangle.Height / 2);
        }


        private void ManageVerticalMovement()
        {
            // Jumping
            jumpPressed = (Keyboard.GetState().IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)) || (Keyboard.GetState().IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W));

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
            if (onGround && jumpPressed && jumpCounter < numJumps)
            {
                velocity.Y -= jumpAmmount;
                jumpCounter++;
            }
            else if (incrementJumps > 0 && jumpPressed)
            {
                velocity.Y -= jumpAmmount;
                incrementJumps--;
            }
        }

        private void HandleHorizontalMovement()
        {
            const float MOVEMENT_SPEED = 3f;
            const float MAX_SPEED = 20f;

            //inputs
            bool movingLeft = Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
            bool movingRight = Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            // Horizontal movement
            if (movingLeft)
            {
                if (!(velocity.X <= -5f))
                {
                    velocity.X += -MOVEMENT_SPEED;
                    lookAhead = -200;
                }
                directionLeft = true;
            }
            if (movingRight)
            {
                if (!(velocity.X >= 5f))
                {
                    velocity.X += MOVEMENT_SPEED;
                    lookAhead = 200;
                }
                directionLeft = false;
            }
            // Dash (sprint)
            if (dashCounter > 0 && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !prevState.IsKeyDown(Keys.LeftShift))
            {
                int basedOnDirection = directionLeft ? -7 : 7;
                velocity.X += basedOnDirection;
                dashCounter--;
                lookAhead += 60 * basedOnDirection;
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
            foreach (Entity entity in entities)
            {
                // if (entity is Collectable collectable && collectable.Destinationrectangle.Intersects(Destinationrectangle))
                // {
                //     collectable.changeThings(this);
                //     deleteCollectables.Add(collectable);
                // }
            }
        }
    }
}