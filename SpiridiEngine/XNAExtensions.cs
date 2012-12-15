/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Net;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class XNAExtensions
    {
        /** Needed in an un-altered SilverSprite
        public static Rectangle Intersect(this Rectangle r1, Rectangle r2)
        {
            Rectangle result;
            result.X = Math.Max(r1.X, r2.X);
            result.Y = Math.Max(r1.Y, r2.Y);
            result.Width = Math.Max(0, Math.Min(r1.X + r1.Width, r2.X + r2.Width) - result.X);
            result.Height = Math.Max(0, Math.Min(r1.Y + r1.Height, r2.Y + r2.Height) - result.Y);
            return result;
        }
        */
    }
}
