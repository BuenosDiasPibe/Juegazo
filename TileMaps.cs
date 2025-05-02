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
        public Dictionary<Vector2, int> tilemap {get;set;} //TODO: add abstraction to make lots of layer tilemap
        private List<Rectangle> sourceRectangles;
        private List<Rectangle> destinationRectangles;
        private Texture2D texture;
        private int scaleTexture;
        private int pixelSize;
        public TileMaps(Texture2D texture, int scaleTexture, int pixelSize) {
            this.texture = texture;
            this.scaleTexture = scaleTexture;
            this.pixelSize = pixelSize;
            tilemap = new();
            sourceRectangles = new();
            destinationRectangles = new();
        }
        public Dictionary<Vector2, int> LoadMap(string filePath){
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filePath);
            string line;
            int y = 0;
            while((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                for(int x = 0; x < parts.Length; x++){
                    if(int.TryParse(parts[x], out int value)){
                        if(value > -1){
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            return result;
        }
        public void BuildRectangles(KeyValuePair<Vector2, int> item) {
            
            destinationRectangles.Add(new Rectangle(
                (int)item.Key.X*scaleTexture,
                (int)item.Key.Y*scaleTexture,
                scaleTexture,
                scaleTexture
            ));
            int x = item.Value % pixelSize;
            int y = item.Value / pixelSize;
            if(item.Value==19) {
                Console.WriteLine(x*pixelSize + " " + y*pixelSize);
            }
            sourceRectangles.Add(new Rectangle(
                x*pixelSize,
                y*pixelSize,
                pixelSize,
                pixelSize
            ));
        }
        //TODO: add update collision method

        public void Draw(SpriteBatch spriteBatch) {
            for(int i = 0; i < tilemap.Count(); i++) {
                BuildRectangles(tilemap.ElementAt(i));
                spriteBatch.Draw(texture, destinationRectangles.ElementAt(i),sourceRectangles.ElementAt(i), Color.White);
            }
        }
    }
}