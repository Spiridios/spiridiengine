/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TextureImage : Image
    {
        private Texture2D image;
        private Color[] pixelData = null;

        public TextureImage(string imageName)
        {
            image = SpiridiGame.ImageManagerInstance.GetImage(imageName);
        }

        public TextureImage(Texture2D image)
        {
            this.image = image;
        }

        protected Texture2D Texture
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public override int Width
        {
            get { return this.image.Width; }
        }

        public override int Height
        {
            get { return this.image.Height; }
        }

        protected override void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            Rectangle sourceRect = source;
            if (sourceRect.IsEmpty)
            {
                sourceRect.Width = this.Width;
                sourceRect.Height = this.Height;
            }
            Rectangle destRect = destination;
            destRect.X += (int)this.Origin.X;
            destRect.Y += (int)this.Origin.Y;
            spriteBatch.Draw(this.image, destRect, sourceRect, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }

        public override Color GetPixel(int x, int y)
        {
            return GetPixel(new Point(x, y));
        }

        public override Color GetPixel(Point point)
        {
            Point texturePoint = point; // SourcePointToTexturePoint(point);
            if (!InBounds(point))
            {
                throw new InvalidOperationException("Pixel out of bounds");
                //return TextureImage.PIXEL_OUT_OF_BOUNDS;
            }
            else
            {
                int offset = texturePoint.X + (texturePoint.Y * this.image.Width);
                return this.PixelData[offset];
            }
        }

        private Color[] PixelData
        {
            get
            {
                if (this.pixelData == null)
                {
                    this.pixelData = new Color[this.image.Width * this.image.Height];
                    this.image.GetData<Color>(this.pixelData);
                }
                return this.pixelData;
            }
        }
    }
}
