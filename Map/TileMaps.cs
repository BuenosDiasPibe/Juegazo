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
        public Dictionary<Vector2, int> tilemap { get; set; }
        protected Texture2D texture;
        protected int scaleTexture;
        protected int pixelSize;
        protected int numberOfTilesPerRow;
        public TileMaps(Texture2D texture, int scaleTexture, int pixelSize, int numberOfTilesPerRow)
        {
            this.texture = texture;
            this.scaleTexture = scaleTexture;
            this.pixelSize = pixelSize;
            this.numberOfTilesPerRow = numberOfTilesPerRow;
            tilemap = new();
        }

        public virtual Dictionary<Vector2, int> LoadMap(string filePath)
        {
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filePath);
            string line;
            int y = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                for (int x = 0; x < parts.Length; x++)
                {
                    if (int.TryParse(parts[x], out int value))
                    {
                        //TODO: change the tile system to not use this magic values, they are for the collectable system but its just garbage
                        if (value > -1)
                        {
                            Vector2 vector = new Vector2(x, y);
                            result[vector] = value;
                        }
                    }
                }
                y++;
            }
            return result;
        }

        public virtual Rectangle BuildSourceRectangle(KeyValuePair<Vector2, int> item)
        {
            int x = item.Value % numberOfTilesPerRow;
            int y = item.Value / numberOfTilesPerRow;
            return new Rectangle(
                x * pixelSize,
                y * pixelSize,
                pixelSize,
                pixelSize
            );
        }
        public virtual Rectangle BuildDestinationRectangle(KeyValuePair<Vector2, int> item)
        {
            return new Rectangle(
                            (int)item.Key.X * scaleTexture,
                            (int)item.Key.Y * scaleTexture,
                            scaleTexture,
                            scaleTexture
                        );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tilemap)
            {
                spriteBatch.Draw(texture, BuildDestinationRectangle(tile), BuildSourceRectangle(tile), Color.White);
            }
        }
    }
}