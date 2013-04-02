using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TileSetCollection
    {
        private List<TileSet> tileImages = new List<TileSet>();
        private SortedList<int, int> tileIdMap = new SortedList<int, int>();
        // How to find the TileSet for the given tileId?

        public void AddTileSet(TileSet tileImage, int startTileId)
        {
            this.tileIdMap.Add(startTileId, this.tileImages.Count);
            this.tileImages.Add(tileImage);
        }

        public Image GetImage(int tileId)
        {
            Image image = null;
            if (tileId > 0)
            {
                int key = FindKey(tileId);
                if (key >= 0)
                {
                    int index = tileIdMap[key];
                    TileSet ti = tileImages[index];
                    image = ti.CreateTileImage(tileId - (key - 1));
                }
            }
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="tileId">The ID of the tile to draw. 0 means don't draw the tile, 1 is the upper-leftmost tile, 2 is the tile to the right of that tile.</param>
        public void DrawTile(SpriteBatch spriteBatch, int tileId, Rectangle destination)
        {
            /* Code to test GetImage
            Image image = GetImage(tileId);
            if (image != null)
            {
                Vector2 position;
                position.Y = destination.Y;
                position.X = destination.X;
                image.Draw(spriteBatch, position);
            }
            // */

            if (tileId > 0)
            {
                int key = FindKey(tileId);
                if (key >= 0 )
                {
                    int index = tileIdMap[key];
                    TileSet ti = tileImages[index];
                    ti.DrawTile(spriteBatch, tileId - (key - 1), destination);
                }
            }
        }

        private int FindKey(int tileId)
        {
            int lastKey = -1;
            foreach (KeyValuePair<int, int> tileIdPair in this.tileIdMap)
            {
                if (tileId > tileIdPair.Key)
                {
                    lastKey = tileIdPair.Key;
                }
                else
                {
                    break;
                }
            }
            return lastKey;
        }
    }
}
