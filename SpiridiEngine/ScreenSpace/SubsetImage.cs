using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Spiridios.SpiridiEngine
{
    public class SubsetImage : Image
    {
        private Image sourceImage;
        private Rectangle sourceRect;

        public SubsetImage(Image image, Rectangle sourceRect)
        {
            this.sourceImage = image;
            this.sourceRect = sourceRect;
        }

        public override Vector2 Origin
        {
            get
            {
                return base.Origin;
            }
            set
            {
                base.Origin = value;
                this.sourceImage.Origin = value;
            }
        }

        public Rectangle SourceRectangle
        {
            get { return this.sourceRect; }
            set { this.sourceRect = value; }
        }

        public override int Width
        {
            get { return sourceRect.Width; }
        }

        public override int Height
        {
            get { return sourceRect.Height; }
        }

        protected override void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            Rectangle tmpSource = source;
            if (tmpSource.IsEmpty)
            {
                tmpSource = this.sourceRect;
            }
            else
            {
                tmpSource.X += this.sourceRect.X;
                tmpSource.Y += this.sourceRect.Y;
            }

            sourceImage.Draw(spriteBatch, tmpSource, destination, tintColor, rotation, layer);
        }

        public override Color GetPixel(int x, int y)
        {
            return this.GetPixel(new Point(x, y));
        }

        public override Color GetPixel(Point point)
        {
            Point texturePoint = SourcePointToTexturePoint(point);
            return sourceImage.GetPixel(texturePoint);
        }

        private Point SourcePointToTexturePoint(Point point)
        {
            Point p = new Point(point.X, point.Y);
            p.X += sourceRect.X;
            p.Y += sourceRect.Y;
            return p;
        }
    }
}
