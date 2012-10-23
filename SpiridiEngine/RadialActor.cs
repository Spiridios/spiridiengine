using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class RadialActor : Actor
    {
        private RadialPositionHandler radialPositionHandler;

        public RadialActor(string imageName) : base(imageName)
        {
            this.radialPositionHandler = new RadialPositionHandler();
            this.PositionHandler = this.radialPositionHandler;
            this.radialPositionHandler.CenterCorrection = this.GetCenterOffset();
        }

        public Vector2 Origin
        {
            get { return radialPositionHandler.Origin; }
            set { radialPositionHandler.Origin = value; }
        }

        public float Theta
        {
            get { return radialPositionHandler.Theta; }
            set { radialPositionHandler.Theta = value; }
        }

        public Vector2 BasePosition
        {
            get { return radialPositionHandler.BasePosition; }
            set { radialPositionHandler.BasePosition = value; }
        }

    }
}
