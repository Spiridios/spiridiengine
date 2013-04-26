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

        public void Center(Vector2 position)
        {
            this.position = position;
            this.position.X = this.position.X - (int)(SpiridiGame.Instance.WindowWidth / 2.0);
            this.position.Y = this.position.Y - (int)(SpiridiGame.Instance.WindowHeight / 2.0);
        }
    }
}
