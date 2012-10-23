using System;
using System.Net;
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
