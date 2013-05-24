﻿/**
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
            if (source.IsEmpty)
            {
                source.Width = this.Width;
                source.Height = this.Height;
            }
            destination.X += (int)this.Origin.X;
            destination.Y += (int)this.Origin.Y;
            spriteBatch.Draw(this.image, destination, source, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }

        public override Color GetPixel(int x, int y)
        {
            if (!InBounds(x,y))
            {
                throw new InvalidOperationException("Pixel out of bounds");
            }
            else
            {
                int offset = x + (y * this.image.Width);
                return this.PixelData[offset];
            }
        }

        public override Color GetPixel(Point point)
        {
            return GetPixel(point.X, point.Y);
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
