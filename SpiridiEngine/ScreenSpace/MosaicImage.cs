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
    public class MosaicImage : Image
    {
        private List<Image> mosaicImages = null;
        // In pixels
        private int imageWidth;
        private int imageHeight;
        // In tiles
        private int mosaicWidth;
        private int mosaicHeight;

        private int tileWidth;
        private int tileHeight;

        public MosaicImage(List<Image> sourceImages, int tileWidth, int tileHeight, int mosaicWidth, int mosaicHeight)
            : base()
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.mosaicWidth = mosaicWidth;
            this.mosaicHeight = mosaicHeight;
            this.imageWidth = tileWidth * mosaicWidth;
            this.imageHeight = tileHeight * mosaicHeight;
            this.mosaicImages = sourceImages;

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
            int size = mosaicImages.Count;
            Rectangle destRect = new Rectangle(0, 0, tileWidth, tileHeight);
            for (int i = 0; i < size; i++)
            {
                Vector2 destCoord = TileSet.GetImageCoordinatesFromOffset(i, mosaicWidth, tileWidth, tileHeight);

                Image image = mosaicImages[i];
                if (image != null)
                {
                    destRect.X = destination.X + (int)destCoord.X;
                    destRect.Y = destination.Y + (int)destCoord.Y;
                    image.Draw(spriteBatch, destRect, tintColor, rotation, layer);
                }
            }

            //spriteBatch.Draw(this.image, destination, sourceRect, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }


        public override Color GetPixel(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override Color GetPixel(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
