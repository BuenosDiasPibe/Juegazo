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
    public class Player : Sprite
    {
        public Vector2 velocity;
        private bool nervioso_la_nena = false;
        Color prevColor;
        Color nervioso = new Color(new Vector3(0.89f, 0.34f, 0.16f));
        float t;
        public bool onGround {get; set;}
        public Player(Texture2D texture, Vector2 position, float scale, Color color) : base(texture, position, scale, color)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.color = color;
            prevColor = color;
            velocity = new();
            onGround=true;
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState, Viewport viewport)
        {
            //TODO: add vertical velocity and gravity
            velocity.X = 0;
            if(keyboardState.IsKeyDown(Keys.Left) && position.X >= texture.Width*scale/2) {
                velocity.X -= 10;
            }
            if (keyboardState.IsKeyDown(Keys.Right) && position.X <= viewport.Width - texture.Width*scale/2) {
                velocity.X += 10;
            }
            if(keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right)) {
                velocity.X = 0;
                nervioso_la_nena = true;
            }
            position.X += velocity.X;
            velocity.Y += 0.2f;
            if(velocity.Y > 10){
                velocity.Y = 10;
            }
            if(position.Y >= viewport.Height-texture.Width*scale/2){
                velocity.Y=0;
            }

            if(keyboardState.IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up)){ //WE BALL!!!!
                velocity.Y = -8;
            } //TODO: add onGround functionality on foreach loop

            
            position.Y += velocity.Y;   

            //silly shit
            if(nervioso_la_nena && t < 1) {
                t += (float)gameTime.ElapsedGameTime.TotalSeconds;
                color = Color.Lerp(nervioso, prevColor, t);
                if (t >= 1) {
                    nervioso_la_nena = false;
                    color = prevColor;
                    t = 0;
                }
            }
        }
    }
}