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
