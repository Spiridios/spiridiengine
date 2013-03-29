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
    public class StaticSprite : Sprite
    {
        private Image image = null;

        public StaticSprite(string imageName)
            : base()
        {
            image = new Image(imageName);
            image.Origin = new Vector2(image.Width / 2.0f, image.Height / 2.0f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch, this.Position, this.TintColor, this.Rotation, this.Layer);
        }

        public override Vector2 CenterOffset
        {
            get { return this.image.Origin; }
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Height
        {
            get { return image.Height; }
        }
    }
}
