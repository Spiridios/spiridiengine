/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine.GUI
{
    public abstract class Window : ScreenObject, Updatable
    {
        private bool drawFrame = false;
        private Image image;

        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        public virtual void Update(TimeSpan elapsedTime) {}
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.image != null)
            {
                image.Draw(spriteBatch, this.Extents);
            }
            if(this.drawFrame)
            {
                SpiridiGame.Instance.DrawRectangle(this.Extents, Color.Black);
            }
        }
    }
}
