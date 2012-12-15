/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Adds helper methods to XNA's Vector2
    /// </summary>
    public class Vector2Ext
    {
        /// <summary>
        /// Rotate the vector by theta radians
        /// </summary>
        /// <param name="v"></param>
        /// <param name="theta">Radians to rotate</param>
        /// <returns></returns>
        public static Vector2 Rotate(Vector2 v, float theta)
        {
            float newX = (v.X * (float)Math.Cos(theta)) - (v.Y * (float)Math.Sin(theta));
            float newY = (v.Y * (float)Math.Cos(theta)) + (v.X * (float)Math.Sin(theta));
            return new Vector2(newX, newY);
        }

        /// <summary>
        /// Rotates the vector around another vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="origin"></param>
        /// <param name="theta">Radians to rotate</param>
        /// <returns></returns>
        public static Vector2 RotateAround(Vector2 v, Vector2 origin, float theta)
        {
            return Vector2.Add(Vector2Ext.Rotate(Vector2.Subtract(v, origin),theta), origin);
        }

    }
}
