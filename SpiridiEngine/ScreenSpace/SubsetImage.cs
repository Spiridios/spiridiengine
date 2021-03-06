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
using System.Collections.Generic;

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

        public SubsetImage(string imageName, Rectangle sourceRect)
            : this(new TextureImage(imageName), sourceRect)
        {
        }

        public override Vector2 Origin
        {
            get { return base.Origin; }
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

        public override void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            if (source.IsEmpty)
            {
                sourceImage.Draw(spriteBatch, this.sourceRect, destination, tintColor, rotation, layer);
            }
            else
            {
                source.X += this.sourceRect.X;
                source.Y += this.sourceRect.Y;
                sourceImage.Draw(spriteBatch, source, destination, tintColor, rotation, layer);
            }
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

        public override Turtle GetTurtle(int x, int y)
        {
            if (!InBounds(x, y))
            {
                throw new InvalidOperationException("Pixel out of bounds");
            }
            else
            {
                return new SubsetTurtle(this, x + sourceRect.X, y + sourceRect.Y);
            }
        }

        private class SubsetTurtle : Turtle
        {
            private int x;
            private int y;
            private SubsetImage image;
            public SubsetTurtle(SubsetImage image, int x, int y)
            {
                this.image = image;
                this.x = x;
                this.y = y;
            }

            public override Color GetPixel()
            {
                return image.sourceImage.GetPixel(x, y);
            }

            public override Color GetNextPixel(Direction direction)
            {
                switch (direction)
                {
                    case (Direction.Down):
                        y++;
                        break;
                    case (Direction.Up):
                        y--;
                        break;
                    case (Direction.Left):
                        x--;
                        break;
                    case (Direction.Right):
                        x++;
                        break;
                }

                return GetPixel();
            }
        }

    }
}
