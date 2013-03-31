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
        private Vector2 radialOrigin;
        private Vector2 screenPosition;
        private float theta;
        private Vector2 objectOrigin;

        public RadialPositionHandler()
        {
            RadialOrigin = new Vector2(0, 0);
            screenPosition = new Vector2(0, 0);
            theta = 0;
            objectOrigin = new Vector2(0, 0);
        }

        public Vector2 RadialOrigin
        {
            get { return radialOrigin; }
            set { radialOrigin = value; }
        }

        public Vector2 CenterCorrection
        {
            set { this.objectOrigin = value; }
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
                RefreshScreenPosition();
            }
        }

        public override Vector2 ScreenPosition
        {
            get { return screenPosition; }
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; RefreshScreenPosition(); }
        }

        private void RefreshScreenPosition()
        {
            screenPosition = Vector2.Add(Vector2.Subtract(Vector2Ext.Rotate(this.Position, this.Theta), this.objectOrigin), this.RadialOrigin);
        }
    }
}
