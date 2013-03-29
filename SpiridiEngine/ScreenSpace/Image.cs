/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class Image : ScreenObject, Updatable
    {
        private Sprite image;

        public Image(string imageName)
            :this(new StaticSprite(imageName))
        {

        }

        public Image(Sprite image)
        {
            this.image = image;
        }

        public Vector2 CenterOffset
        {
            get { return this.image.CenterOffset; }
        }

        public override int Width
        {
            get { return this.image.Width; }
        }

        public override int Height
        {
            get { return this.image.Height; }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.image.Draw(spriteBatch, this.Position);
        }

        public void Update(TimeSpan elapsedTime)
        {
            this.image.Update(elapsedTime);
        }
    }
}
