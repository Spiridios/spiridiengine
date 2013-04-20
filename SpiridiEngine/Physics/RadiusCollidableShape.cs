/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine.Physics
{
    public class RadiusCollidableShape
    {
        private Vector2 position;
        private double boundingRadius = -1.0f;

        public RadiusCollidableShape(Vector2 position, double boundingRadius)
        {
            this.position = position;
            this.boundingRadius = boundingRadius;
        }

        public RadiusCollidableShape(double boundingRadius)
            : this(new Vector2(0,0), boundingRadius)

        {
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public double BoundingRadius
        {
            get { return this.boundingRadius; }
            set { this.boundingRadius = value; }
        }
    }
}
