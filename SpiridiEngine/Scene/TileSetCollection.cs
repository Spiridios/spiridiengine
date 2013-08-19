/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TileSetCollection
    {
        private class TileSetCollectionEntry
        {
            public TileSetCollectionEntry(int firstCollectionTileId, TileSet tileSet)
            {
                this.firstCollectionTileId = firstCollectionTileId;
                this.tileSet = tileSet;
            }

            public int firstCollectionTileId;
            public TileSet tileSet;
            public int LastCollectionTileId
            {
                get { return firstCollectionTileId + (tileSet.TileCount - 1); }
            }
        }

        private List<TileSetCollectionEntry> tileSets = new List<TileSetCollectionEntry>();

        // collection id -> Image map
        private Dictionary<int, SubsetImage> tileImageMap = new Dictionary<int, SubsetImage>();

        // How to find the TileSet for the given tileId?

        public void AddTileSet(TileSet tileImage, int firstCollectionTileId)
        {
            this.tileSets.Add(new TileSetCollectionEntry(firstCollectionTileId, tileImage));
        }

        public SubsetImage GetImage(int collectionTileId)
        {
            SubsetImage image = null;
            if (this.tileImageMap == null)
            {
                image = this.GetImageNoCache(collectionTileId);
            }
            else if (!this.tileImageMap.TryGetValue(collectionTileId, out image))
            {
                image = this.GetImageNoCache(collectionTileId);
                this.tileImageMap[collectionTileId] = image;
            }
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionTileId">Must be greater than 0</param>
        /// <returns></returns>
        private SubsetImage GetImageNoCache(int collectionTileId)
        {
            SubsetImage image = null;
            TileSetCollectionEntry globalTileSet = FindTileSetEntry(collectionTileId);
            if (globalTileSet != null)
            {
                TileSet ti = globalTileSet.tileSet;
                image = ti.CreateTileImage(collectionTileId - (globalTileSet.firstCollectionTileId - 1));
            }
            return image;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="collectionTileId">The ID of the tile to draw. 0 means don't draw the tile, 1 is the upper-leftmost tile, 2 is the tile to the right of that tile.</param>
        public void DrawTile(SpriteBatch spriteBatch, int collectionTileId, Rectangle destination)
        {
            /* Code to test GetImage
            Image image = GetImage(tileId);
            if (image != null)
            {
                image.Draw(spriteBatch, destination);
            }
            // */

            if (collectionTileId > 0)
            {
                TileSetCollectionEntry globalTileSet = FindTileSetEntry(collectionTileId);
                if (globalTileSet != null)
                {
                    TileSet ti = globalTileSet.tileSet;
                    ti.DrawTile(spriteBatch, collectionTileId - (globalTileSet.firstCollectionTileId - 1), destination);
                }
            }
        }

        private TileSetCollectionEntry FindTileSetEntry(int collectionTileId)
        {
            foreach (TileSetCollectionEntry entry in this.tileSets)
            {
                if (collectionTileId >= entry.firstCollectionTileId && collectionTileId <= entry.LastCollectionTileId)
                {
                    return entry;
                }
            }
            return null;
        }
    }
}
