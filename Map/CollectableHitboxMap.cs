using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarinMol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juegazo
{
    public class CollectableHitboxMap : TileMaps
    {
        private float collectableSize;
        private int[] collectableValues;

        public CollectableHitboxMap(Texture2D texture, int scaleTexture, int pixelSize, int numberOfTilesPerRow, float collectableSize) : base(texture, scaleTexture, pixelSize, numberOfTilesPerRow)
        {
            this.collectableSize = collectableSize;
            collectableValues = [13, 14];
            this.scaleTexture = scaleTexture;
        }
        public override Dictionary<Vector2, int> LoadMap(string filePath)
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
                        foreach (int collectableValue in collectableValues)
                        {
                            if (value == collectableValue)
                            {
                                Vector2 vector = new Vector2(x, y);
                                result[vector] = value;
                            }
                        }
                    }
                }
                y++;
            }
            return result;
        }
        public override Rectangle BuildDestinationRectangle(KeyValuePair<Vector2, int> item)
        {
            return new Rectangle(
                            (int)((item.Key.X * scaleTexture) + scaleTexture*(collectableSize*collectableSize)),
                            (int)((item.Key.Y * scaleTexture) + scaleTexture*(collectableSize*collectableSize)),
                            (int)(scaleTexture * collectableSize),
                            (int)(scaleTexture * collectableSize)
            );
        }
        public override Rectangle BuildSourceRectangle(KeyValuePair<Vector2, int> item)
        {
            int x = item.Value % numberOfTilesPerRow;
            int y = item.Value / numberOfTilesPerRow;
            return new Rectangle(
                            (int)(x * pixelSize + pixelSize*collectableSize*collectableSize),
                            (int)(y * pixelSize + pixelSize*collectableSize*collectableSize),
                            (int)(pixelSize*collectableSize),
                            (int)(pixelSize*collectableSize)
                        );
        }
    }
}