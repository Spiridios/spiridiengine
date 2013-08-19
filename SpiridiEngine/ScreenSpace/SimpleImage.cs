using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    class SimpleImage : PixelProvider
    {
        private Texture2D image;
        private Color[] pixelData = null;
        private Vector2 origin = new Vector2(0, 0);

        public SimpleImage(string imageName)
        {
            image = SpiridiGame.ImageManagerInstance.GetImage(imageName);
        }

        public SimpleImage(Texture2D image)
        {
            this.image = image;
        }

        public virtual Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public int Width
        {
            get { return this.image.Width; }
        }

        public int Height
        {
            get { return this.image.Height; }
        }

        protected void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
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

        public Color GetPixel(int x, int y)
        {
            if (!InBounds(x, y))
            {
                throw new InvalidOperationException("Pixel out of bounds");
            }
            else
            {
                int offset = x + (y * this.image.Width);
                return this.PixelData[offset];
            }
        }

        public Color GetPixel(Point point)
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

        private bool InBounds(int x, int y)
        {
            return (x >= 0 && x < this.Width && y >= 0 && y < this.Height);
        }
    }
}
