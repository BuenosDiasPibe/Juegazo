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
        public Player(Texture2D texture, Vector2 position, float scale, Color color) : base(texture, position, scale, color)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.color = color;
            prevColor = color;
            velocity = new();
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevState)
        {
            //TODO: add vertical velocity and gravity
            velocity.X = 0;
            if(keyboardState.IsKeyDown(Keys.Left))
            {
                velocity.X = -5;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                velocity.X = 5;
            }if(keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
            {
                velocity.X = 0;
                nervioso_la_nena = true;
            }

            //silly shit
            if(nervioso_la_nena && t < 1)
            {
                t += (float)gameTime.ElapsedGameTime.TotalSeconds;
                color = Color.Lerp(nervioso, prevColor, t);

                if (t >= 1)
                {
                    nervioso_la_nena = false;
                    color = prevColor;
                    t = 0;
                }
            }
        }
    }
}