/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TileMapImage : Image
    {
        private List<Image> tileImages = null;
        // In pixels
        private int imageWidth;
        private int imageHeight;
        // In tiles
        private int mapWidth;
        private int mapHeight;

        private int tileWidth;
        private int tileHeight;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileImages">A list of images that make up the tiles.
        /// If an entry is null, it is treated as an image that is filled with transparent pixels.</param>
        /// <param name="tileWidth">Width of the tiles</param>
        /// <param name="tileHeight">Height of the tiles</param>
        /// <param name="mapWidth">Map width in tiles</param>
        /// <param name="mapHeight">Map height in tiles</param>
        public TileMapImage(List<Image> tileImages, int tileWidth, int tileHeight, int mapWidth, int mapHeight)
            : base()
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.imageWidth = tileWidth * mapWidth;
            this.imageHeight = tileHeight * mapHeight;
            this.tileImages = tileImages;

        }

        public override int Width
        {
            get { return imageWidth; }
        }

        public override int Height
        {
            get { return imageHeight; }
        }

        protected override void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            int size = tileImages.Count;
            float scaleFactorX = destination.Width / (float)imageWidth;
            float scaleFactorY = destination.Height / (float)imageHeight;
            int scaledWidth = (int)(tileWidth * scaleFactorX);
            int scaledHeight = (int)(tileHeight * scaleFactorY);

            Rectangle destRect = new Rectangle(0, 0, scaledWidth, scaledHeight);
            for (int i = 0; i < size; i++)
            {
                Vector2 destCoord = TileSet.GetImageCoordinatesFromOffset(i, mapWidth, scaledWidth, scaledHeight);

                Image image = tileImages[i];
                if (image != null)
                {
                    destRect.X = destination.X + (int)destCoord.X;
                    destRect.Y = destination.Y + (int)destCoord.Y;
                    image.Draw(spriteBatch, destRect, tintColor, rotation, layer);
                }
            }
        }


        public override Color GetPixel(int x, int y)
        {
            Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            if (!InBounds(x, y))
            {
                throw new InvalidOperationException("Pixel out of bounds");
            }
            else
            {
                int tileX = x / tileWidth;
                int tileY = y / tileHeight;
                int index = tileY * this.mapWidth + tileX;

                // Note: if X and Y are out of bounds, then this may also produce an out of bounds exception.
                Image image = this.tileImages[index];
                if (image != null)
                {
                    int tilePixelX = x - (tileX * tileWidth);
                    int tilePixelY = y - (tileY * tileHeight);
                    color = image.GetPixel(tilePixelX, tilePixelY);
                }
            }
            return color;
        }

        public override Color GetPixel(Point point)
        {
            return GetPixel(point.X, point.Y);
        }
    }
}
