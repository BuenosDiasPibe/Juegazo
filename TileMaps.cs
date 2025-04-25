using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarinMol
{
    public class TileMaps
    {
        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureStore;
        private Texture2D texture;
        TileMaps(Dictionary<Vector2, int> tilemap, List<Rectangle> textureStore, Texture2D texture) {
            this.tilemap = tilemap;
            this.textureStore = textureStore;
            this.texture = texture;
        }
        private Dictionary<Vector2, int> LoadMap(string filePath){
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filePath);
            string line;
            int y = 0;
            while((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                for(int x = 0; x < parts.Length; x++){
                    if(int.TryParse(parts[x], out int value)){
                        if(value > 0){
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            return result;
        }

        public void Draw(SpriteBatch spriteBatch, int scale) {
            foreach(var tile in tilemap)
            {
                Rectangle destination = new Rectangle(
                    (int)(tile.Key.X*scale),
                    (int)(tile.Key.Y*scale),
                    scale,
                    scale
                );
                Rectangle soruceRectangle = textureStore[tile.Value-1];
                spriteBatch.Draw(texture, destination, soruceRectangle, Color.White);
        }
        }
    }
}