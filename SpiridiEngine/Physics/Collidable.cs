/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class Collidable
    {
        private RadiusCollidableShape radiusShape = null;
        private bool confineNormals = false;

        public bool OrthoVectors
        {
            get { return confineNormals; }
            set { confineNormals = value; }
        }

        public RadiusCollidableShape RadiusCollidableShape
        {
            get { return this.radiusShape; }
            set { this.radiusShape = value; }
        }

        public bool CollidesWith(Collidable that)
        {
            if (this.RadiusCollidableShape!= null && that.RadiusCollidableShape != null)
            {
                return CollidesWithRadiusRadius(that);
            }
            else
            {
                return false;
            }
        }

        public Vector2 CollisionNormal(Collidable that)
        {
            if (this.RadiusCollidableShape != null && that.RadiusCollidableShape != null)
            {
                return CollisionNormalRadiusRadius(that);
            }
            else
            {
                return Vector2.Zero;
            }
        }


        public Vector2 CollisionVector(Collidable that)
        {
            if (this.RadiusCollidableShape != null && that.RadiusCollidableShape != null)
            {
                return CollisionVectorRadiusRadius(that);
            }
            else
            {
                return Vector2.Zero;
            }
        }
        
        private bool CollidesWithRadiusRadius(Collidable that)
        {
            Vector2 direction = this.radiusShape.Position - that.radiusShape.Position;
            float length = direction.Length();
            double thisRadius = this.radiusShape.BoundingRadius;
            double thatRadius = that.radiusShape.BoundingRadius;
            return !(length > (thisRadius + thatRadius));
        }

        private Vector2 CollisionNormalRadiusRadius(Collidable that)
        {
            Vector2 direction = this.radiusShape.Position - that.radiusShape.Position;
            float length = direction.Length();
            double thisRadius = this.radiusShape.BoundingRadius;
            double thatRadius = that.radiusShape.BoundingRadius;
            if(!(length > (thisRadius + thatRadius)))
            {
                direction.Normalize();
                return direction;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private Vector2 CollisionVectorRadiusRadius(Collidable that)
        {
            Vector2 direction = this.radiusShape.Position - that.radiusShape.Position;
            float length = direction.Length();
            double thisRadius = this.radiusShape.BoundingRadius;
            double thatRadius = that.radiusShape.BoundingRadius;
            if (!(length > (thisRadius + thatRadius)))
            {
                if (confineNormals)
                {
                    direction = orthoizeVector(direction);
                }
                return direction;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private Vector2 orthoizeVector(Vector2 vector)
        {
            float length = vector.Length();
            if (Math.Abs(vector.X) > Math.Abs(vector.Y))
            {
                vector.Y = 0;
            }
            else
            {
                vector.X = 0;
            }
            vector.Normalize();
            vector = vector * length;
            return vector;
        }

    }
}
