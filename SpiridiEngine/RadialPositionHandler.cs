using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class RadialPositionHandler : PositionHandler
    {
        public Vector2 Origin { get; set; }
        private Vector2 untransformedPosition;
        private float theta;
        private Vector2 centerCorrection;

        public RadialPositionHandler()
        {
            Origin = new Vector2(0, 0);
            untransformedPosition = new Vector2(0, 0);
            theta = 0;
            centerCorrection = new Vector2(0, 0);
        }

        public Vector2 CenterCorrection
        {
            set { this.centerCorrection = value; }
        }

        public float Theta
        {
            get { return theta; }
            set
            {
                theta = value;
                if (theta > (float)(Math.PI * 2.0))
                {
                    theta -= (float)(Math.PI * 2.0);
                }
                else if (theta < 0)
                {
                    theta += (float)(Math.PI * 2.0);
                }
                RefreshPosition();
            }
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; this.untransformedPosition = value; this.theta = 0; }
        }

        public Vector2 BasePosition
        {
            get { return this.untransformedPosition; }
            set { this.untransformedPosition = value; RefreshPosition(); }
        }

        private void RefreshPosition()
        {
            base.Position = this.GetRotatedPosition();
        }

        private Vector2 GetRotatedPosition()
        {
            return Vector2.Subtract(Vector2Ext.RotateAround(Vector2.Add(this.untransformedPosition, this.centerCorrection), this.Origin, this.Theta), this.centerCorrection);
        }
    }
}
