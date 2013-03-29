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
    public class Image : DrawableAt
    {
        private Texture2D image;
        private Vector2 origin = new Vector2(0, 0);

        public Image(string imageName)
        {
            image = SpiridiGame.ImageManagerInstance.GetImage(imageName);
        }

        public Image(Texture2D image)
        {
            this.image = image;
        }

        // TODO: these do not seem like they belong here.
        public Color TintColor { get; set; }
        public float Layer { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle destRect;
            destRect.X = (int)(position.X);
            destRect.Y = (int)(position.Y);
            destRect.Width = (int)(this.image.Width);
            destRect.Height = (int)(this.image.Height);

            spriteBatch.Draw(this.image, destRect, null, TintColor, Rotation, Origin, SpriteEffects.None, Layer);
        }
    }
}
