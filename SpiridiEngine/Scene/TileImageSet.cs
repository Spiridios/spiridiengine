using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TileImageSet
    {
        private List<TileImage> tileImages = new List<TileImage>();
        private SortedList<int, int> tileIdMap = new SortedList<int, int>();
        // How to find the TileImage for the given tileId?

        public void AddImage(TileImage tileImage, int startTileId)
        {
            this.tileIdMap.Add(startTileId, this.tileImages.Count);
            this.tileImages.Add(tileImage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="tileId">The ID of the tile to draw. 0 means don't draw the tile, 1 is the upper-leftmost tile, 2 is the tile to the right of that tile.</param>
        public void DrawTile(SpriteBatch spriteBatch, int tileId, Rectangle destination)
        {
            if (tileId > 0)
            {
                int key = FindKey(tileId);
                if (key >= 0 )
                {
                    int index = tileIdMap[key];
                    TileImage ti = tileImages[index];
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
