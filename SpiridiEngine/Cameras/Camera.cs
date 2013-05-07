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

        public Vector2 TranslatePoint(Vector2 point)
        {
            return point - halfWindowPosition;
        }

        public Point TranslatePoint(Point point)
        {
            point.X -= (int)halfWindowPosition.X;
            point.Y -= (int)halfWindowPosition.Y;
            return point;
        }

        public Rectangle TranslateRectangle(Rectangle rect)
        {
            rect.X -= (int)halfWindowPosition.X;
            rect.Y -= (int)halfWindowPosition.Y;

            return rect;
        }
    }
}
