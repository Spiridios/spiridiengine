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
    public abstract class Image : PixelProvider
    {
        private Vector2 origin = new Vector2(0, 0);

        protected Image()
        {
        }

        public virtual Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            this.Draw(spriteBatch, position, Color.White, 0.0f, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            this.Draw(spriteBatch, position, Color.White, 0.0f, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tintColor, float rotation, float layer)
        {
            // TODO: I'm not sure if origin should be factoered in here or not. The underlying XNA call expects origin to NOT be in the rect.
            Rectangle destination;
            destination.X = (int)(position.X - origin.X);
            destination.Y = (int)(position.Y - origin.Y);
            destination.Width = this.Width;
            destination.Height = this.Height;

            this.Draw(spriteBatch, destination, tintColor, rotation, layer);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            this.Draw(spriteBatch, Rectangle.Empty, destination, tintColor, rotation, layer);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            this.DrawImpl(spriteBatch, source, destination, tintColor, rotation, layer);
        }

        protected abstract void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer);

        public abstract Color GetPixel(int x, int y);

        public abstract Color GetPixel(Point point);

        protected bool InBounds(int x, int y)
        {
            return (x >= 0 && x < this.Width && y >= 0 && y < this.Height);
        }

        protected bool InBounds(Point point)
        {
            return InBounds(point.X, point.Y);
        }

    }
}
