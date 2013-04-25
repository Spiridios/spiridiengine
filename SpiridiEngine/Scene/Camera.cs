using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine.Scene
{
    public class Camera
    {
        private Vector2 position = Vector2.Zero;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }

        }

        public Vector2 TranslatePoint(Vector2 point)
        {
            return point - position;
        }
    }
}
