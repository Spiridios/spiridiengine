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
    public class Image 
    {
        private Texture2D image;
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
            Rectangle destRect;
            destRect.X = (int)(position.X + origin.X);
            destRect.Y = (int)(position.Y + origin.Y);
            destRect.Width = (int)(sourceRect.Width);
            destRect.Height = (int)(sourceRect.Height);

            spriteBatch.Draw(this.image, destRect, sourceRect, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }
    }
}
