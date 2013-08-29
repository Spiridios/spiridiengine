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
    /// <summary>
    /// Represents an object on screen
    /// </summary>
    public abstract class ScreenObject : Drawable
    {
        private Rectangle extents;

        public ScreenObject()
        {
        }

        public virtual Vector2 Position
        {
            get { return new Vector2(extents.X, extents.Y); }
            set
            { 
                extents.X = (int)value.X;
                extents.Y = (int)value.Y;
            }
        }

        protected Rectangle Extents
        {
            get { return extents; }
            set { extents = value; }
        }

        public int X
        {
            get { return extents.X; }
            set { extents.X = value; }
        }

        public int Y
        {
            get { return extents.Y; }
            set { extents.Y = value; }
        }

        public virtual int Width
        {
            get { return extents.Width; }
            set { extents.Width = value; }
        }

        public virtual int Height
        {
            get { return extents.Height; }
            set { extents.Height = value; }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
