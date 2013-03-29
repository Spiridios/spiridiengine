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
        public ScreenObject()
        {
        }

        public virtual Vector2 Position { get; set; }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
