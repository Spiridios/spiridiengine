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
    public class Image 
    {
        private static readonly Color PIXEL_OUT_OF_BOUNDS = new Color(0, 0, 0, 0);

        private Texture2D image;
        private Color[] pixelData = null;
        private Vector2 origin = new Vector2(0, 0);
        private Rectangle sourceRect;

        public Image(string imageName)
        {
            image = SpiridiGame.ImageManagerInstance.GetImage(imageName);
            sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public Image(Texture2D image)
        {
            this.image = image;
            sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public Image(string imageName, Rectangle sourceRect)
        {
            this.sourceRect = sourceRect;
        }

        public Image(Texture2D image, Rectangle sourceRect)
        {
            this.image = image;
            this.sourceRect = sourceRect;
        }

        protected Image()
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

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public virtual int Width
        {
            get { return this.image.Width; }
        }

        public virtual int Height
        {
            get { return this.image.Height; }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            this.Draw(spriteBatch, position, Color.White, 0.0f, 1.0f);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color tintColor, float rotation, float layer)
        {
            Rectangle destination;
            destination.X = (int)(position.X + origin.X);
            destination.Y = (int)(position.Y + origin.Y);
            destination.Width = (int)(sourceRect.Width);
            destination.Height = (int)(sourceRect.Height);

            spriteBatch.Draw(this.image, destination, sourceRect, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination, Color tintColor, float rotation, float layer)
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

        public Color GetPixel(int x, int y)
        {
            return GetPixel(new Point(x, y));
        }

        public Color GetPixel(Point point)
        {
            Point texturePoint = SourcePointToTexturePoint(point);
            if (!InBounds(point))
            {
                //throw new Exception("Pixel out of bounds");
                //System.Diagnostics.Debug.Print("Pixel out of bounds");
                return Image.PIXEL_OUT_OF_BOUNDS;
            }
            else
            {
                // TODO: Make this PixelData[offset] instead of using temp var
                Color[] pixelData = this.PixelData;
                int offset = texturePoint.X + (texturePoint.Y * this.image.Width);
                return pixelData[offset];
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
