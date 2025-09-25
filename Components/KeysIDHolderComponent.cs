using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo.Components
{
    public class KeysIDHolderComponent : Component
    {
        public List<uint> keyHolder { get; protected set; } = new();
        public KeysIDHolderComponent()
        {
            keyHolder = new();
            this.EnableUpdate = false;
            this.EnableDraw = false;
        }
        public bool isKeyOnPlayer(uint keyID)
        {
            return keyHolder.Contains(keyID);
        }
        public uint GetKey(uint key)
        {
            keyHolder.Add(key);
            return key;
        }
        public override void Destroy()
        {
            Console.WriteLine("las llaves se destruyen tambien pipipi");
            keyHolder = new();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        { }
    }
}