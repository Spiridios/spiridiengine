using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine.Scene
{
    public class FollowingCamera : Camera, Updatable
    {
        private WorldObject objectToFollow;
        private int margin = 200;

        public FollowingCamera(WorldObject objectToFollow)
        {
            this.objectToFollow = objectToFollow;
        }

        public int Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        

        public void Update(System.TimeSpan elapsedTime)
        {
            Vector2 screenPosition = objectToFollow.ScreenPosition;

            if (screenPosition.X < margin)
            {
                Vector2 v = this.Position;
                v.X = v.X - (margin - screenPosition.X);
                this.Position = v;
            }
            else if (screenPosition.X > SpiridiGame.Instance.WindowWidth - margin)
            {
                Vector2 v = this.Position;
                v.X = v.X + (screenPosition.X - (SpiridiGame.Instance.WindowWidth - margin));
                this.Position = v;
            }

            if (screenPosition.Y < margin)
            {
                Vector2 v = this.Position;
                v.Y = v.Y - (margin - screenPosition.Y);
                this.Position = v;
            }
            else if (screenPosition.Y > SpiridiGame.Instance.WindowHeight - margin)
            {
                Vector2 v = this.Position;
                v.Y = v.Y + (screenPosition.Y - (SpiridiGame.Instance.WindowHeight - margin));
                this.Position = v;
            }

        }

    }
}
