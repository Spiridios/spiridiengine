/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

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

        public Vector2 RadialOrigin
        {
            get { return radialPositionHandler.RadialOrigin; }
            set { radialPositionHandler.RadialOrigin = value; }
        }

        // TODO: PositionHandlers need to notify ScreenObjects, not WorldObject notifying ScreenObject
        public float Theta
        {
            get { return radialPositionHandler.Theta; }
            set { radialPositionHandler.Theta = value; this.Sprite.Position = this.PositionHandler.ScreenPosition; }
        }
    }
}
