/**
    Copyright 2012 Micah Lieske

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
    public class Sprite : ScreenObject, Updatable
    {
        protected Image image = null;
        private Updatable updatable = null;

        public Color TintColor { get; set; }
        public float Layer { get; set; }
        public float Rotation { get; set; }

        public Sprite(Image image)
        {
            this.image = image;
            if (image is Updatable)
            {
                this.updatable = (Updatable)image;
            }
            this.image.Origin = new Vector2(image.Width / 2.0f, image.Height / 2.0f);
            TintColor = Color.White;
            Layer = 0;
            Rotation = 0;
        }

        public Sprite(string imageName)
            : this(new Image(imageName))
        {
        }

        public Image Image
        {
            get { return image; }
        }

        public Vector2 CenterOffset
        {
            get { return image.Origin; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch, this.Position, this.TintColor, this.Rotation, this.Layer);
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Height
        {
            get { return image.Height; }
        }

        public virtual void Update(System.TimeSpan elapsedTime)
        {
            if (updatable != null)
            {
                updatable.Update(elapsedTime);
            }
        }

    }
}
