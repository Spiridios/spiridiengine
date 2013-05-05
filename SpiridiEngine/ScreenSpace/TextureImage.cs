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
        private static readonly Color PIXEL_OUT_OF_BOUNDS = new Color(0, 0, 0, 0);

        private Texture2D image;
        private Color[] pixelData = null;
        private Rectangle sourceRect;

        public TextureImage(string imageName)
        {
            image = SpiridiGame.ImageManagerInstance.GetImage(imageName);
            sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public TextureImage(Texture2D image)
        {
            this.image = image;
            sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public TextureImage(string imageName, Rectangle sourceRect)
        {
            this.sourceRect = sourceRect;
        }

        public TextureImage(Texture2D image, Rectangle sourceRect)
        {
            this.image = image;
            this.sourceRect = sourceRect;
        }

        protected TextureImage()
        {
        }

        public Rectangle SourceRectangle
        {
            get { return sourceRect; }
            set { sourceRect = value; }
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

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            this.Draw(spriteBatch, position, Color.White, 0.0f, 1.0f);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, Color tintColor, float rotation, float layer)
        {
            Rectangle destination;
            destination.X = (int)(position.X + Origin.X);
            destination.Y = (int)(position.Y + Origin.Y);
            destination.Width = (int)(sourceRect.Width);
            destination.Height = (int)(sourceRect.Height);

            this.Draw(spriteBatch, destination, tintColor, rotation, layer);
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            spriteBatch.Draw(this.image, destination, sourceRect, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }

        private bool InBounds(int x, int y)
        {
            return (x >= 0 && x < sourceRect.Right && y >= 0 && y < sourceRect.Bottom);
        }

        private bool InBounds(Point point)
        {
            return InBounds(point.X, point.Y);
        }

        public override Color GetPixel(int x, int y)
        {
            return GetPixel(new Point(x, y));
        }

        public override Color GetPixel(Point point)
        {
            Point texturePoint = SourcePointToTexturePoint(point);
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

        private Point SourcePointToTexturePoint(Point point)
        {
            Point p = new Point(point.X, point.Y);
            p.X += sourceRect.X;
            p.Y += sourceRect.Y;
            return p;
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
