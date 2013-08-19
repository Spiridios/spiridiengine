using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine.Scene
{
    public class Camera
    {
        private Vector2 position = Vector2.Zero;
        private Vector2 halfWindowPosition;
        private Vector2 halfWindowExtents;

        public Camera()
        {
            halfWindowExtents = new Vector2((int)(SpiridiGame.Instance.WindowWidth / 2.0), (int)(SpiridiGame.Instance.WindowHeight / 2.0));
        }

        /// <summary>
        /// Sets the world position that the center of the camera focuses on.
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                this.halfWindowPosition = position - halfWindowExtents;
            }

        }

        public void TranslateReference(ref Vector2 point)
        {
            point.X = point.X - halfWindowPosition.X;
            point.Y = point.Y - halfWindowPosition.Y;
        }

        public void TranslateReference(ref Rectangle rect)
        {
            rect.X = (int)(rect.X - halfWindowPosition.X);
            rect.Y = (int)(rect.Y - halfWindowPosition.Y);
        }

        public Vector2 TranslateVector2(Vector2 point)
        {
            return point - halfWindowPosition;
        }

        public Vector2 TranslateVector2(int x, int y)
        {
            return new Vector2(x - halfWindowPosition.X, y - halfWindowPosition.Y);
        }

        public Point TranslatePoint(Point point)
        {
            point.X = point.X - (int)halfWindowPosition.X;
            point.Y = point.Y - (int)halfWindowPosition.Y;
            return point;
        }

        public Rectangle TranslateRectangle(Rectangle rect)
        {
            rect.X = rect.X - (int)halfWindowPosition.X;
            rect.Y = rect.Y - (int)halfWindowPosition.Y;

            return rect;
        }
    }
}
