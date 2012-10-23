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
