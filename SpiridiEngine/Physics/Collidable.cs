/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Spiridios.SpiridiEngine.Physics
{
    public class Collidable
    {
        private CollisionListener collisionListener = null;
        private RadiusCollidableShape radiusShape = null;
        private BoxCollidableShape boxShape = null;
        private ImageCollidableShape imageShape = null;

        // TODO: this shouldn't be needed anymore.
        private bool confineNormals = false;

        // TODO: this shouldn't be needed anymore.
        public bool OrthoVectors
        {
            get { return confineNormals; }
            set { confineNormals = value; }
        }

        public void AddCollisionListener(CollisionListener collisionListener)
        {
            this.collisionListener = collisionListener;
        }

        public void NotifyCollisionListeners(List<Collidable> activeCollidables)
        {
            if (this.collisionListener != null)
            {
                this.collisionListener.OnCollided(activeCollidables);
            }
        }

        public BoxCollidableShape BoxCollidableShape
        {
            get { return this.boxShape; }
            set { this.boxShape = value; }
        }

        public bool HasBoxCollidableShape
        {
            get { return this.boxShape != null; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                if (HasBoxCollidableShape)
                {
                    return boxShape.Rectangle;
                }
                else if (HasRadiusCollidableShape)
                {
                    int width = (int)(radiusShape.BoundingRadius * 2);
                    double offset = radiusShape.BoundingRadius;
                    return new Rectangle((int)(radiusShape.Position.X - offset), (int)(radiusShape.Position.Y - offset), width, width);
                }
                else
                {
                    return new Rectangle(0, 0, 0, 0);
                }
            }
        }

        public RadiusCollidableShape RadiusCollidableShape
        {
            get { return this.radiusShape; }
            set { this.radiusShape = value; }
        }

        public bool HasRadiusCollidableShape
        {
            get { return this.radiusShape != null; }
        }

        public ImageCollidableShape ImageCollidableShape
        {
            get { return this.imageShape; }
            set { this.imageShape = value; }
        }

        public bool HasImageCollidableShape
        {
            get { return this.imageShape != null; }
        }

        public bool CollidesWith(Collidable that)
        {
            Vector2 v = CollisionVector(that);
            return v != Vector2.Zero;
        }

        public Vector2 CollisionNormal(Collidable that)
        {
            Vector2 normal = CollisionVector(that);
            if (normal != Vector2.Zero)
            {
                normal.Normalize();
            }
            return normal;
        }


        public Vector2 CollisionVector(Collidable that)
        {
            Vector2 v = Vector2.Zero;
            if (this.HasBoxCollidableShape && that.HasImageCollidableShape)
            {
                v = CollisionVectorBoxImage(that);
            }
            else if (this.radiusShape != null && that.radiusShape != null)
            {
                v = CollisionVectorRadiusRadius(that);
            }
            else if (this.boxShape != null && that.boxShape != null)
            {
                v = CollisionVectorBoxBox(that);
            }
            else if (this.radiusShape != null && that.boxShape != null)
            {
                v = CollisionVectorRadiusBox(that);
            }
            else if (this.boxShape != null && that.radiusShape != null)
            {
                v = CollisionVectorRadiusBox(this);
                v = v * -1;
            }

            if(confineNormals && v != Vector2.Zero)
            {
                v = Vector2Ext.SnapToAxis(v);
            }

            return v;
        }

        private Vector2 CollisionVectorRadiusRadius(Collidable that)
        {
            Vector2 direction = this.radiusShape.Position - that.radiusShape.Position;
            float length = direction.Length();
            double thisRadius = this.radiusShape.BoundingRadius;
            double thatRadius = that.radiusShape.BoundingRadius;
            if (!(length > (thisRadius + thatRadius)))
            {
                return direction;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private Vector2 CollisionVectorBoxBox(Collidable that)
        {
            Rectangle r = Rectangle.Intersect(this.boxShape.Rectangle, that.boxShape.Rectangle);
            if (r.Width == 0 || r.Height == 0)
            {
                return Vector2.Zero;
            }
            else
            {
                Vector2 v = Vector2.Zero;
                if(that.boxShape.Rectangle.Top == r.Top)
                {
                    v.Y -= 1;
                }

                if(that.boxShape.Rectangle.Bottom == r.Bottom)
                {
                    v.Y += 1;
                }

                if(that.boxShape.Rectangle.Left == r.Left)
                {
                    v.X -= 1;
                }

                if(that.boxShape.Rectangle.Right == r.Right)
                {
                    v.X += 1;
                }

                if (v == Vector2.Zero)
                {
                    //TODO: Arbitrary for now.
                    v = new Vector2(1, 0); 
                }
                return v;
            }
        }

        private Vector2 CollisionVectorRadiusBox(Collidable that)
        {
            Vector2 circle = this.radiusShape.Position;
            double circleRad = this.radiusShape.BoundingRadius;
            Rectangle rect = that.boxShape.Rectangle;
            Vector2 rectCenter = new Vector2(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
            bool collided = false;
            Vector2 v = Vector2.Zero;

            if (rect.Contains((int)circle.X, (int)circle.Y))
            {
                collided = true;
            }
            else
            {
                // Second answer on http://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection

                Vector2 circleDistance = new Vector2(Math.Abs(circle.X - rectCenter.X), Math.Abs(circle.Y - rectCenter.Y));

                if (circleDistance.X <= (rect.Width / 2.0 + circleRad) && circleDistance.Y <= (rect.Height / 2.0 + circleRad))
                {
                    if (circleDistance.X <= (rect.Width / 2.0))
                    { 
                        collided = true;
                    }
                    else if (circleDistance.Y <= (rect.Height / 2.0))
                    {
                        collided = true;
                    }
                    else
                    {
                        double cornerDistance_sq = (circleDistance.X - rect.Width / 2.0) * (circleDistance.X - rect.Width / 2.0) +
                                             (circleDistance.Y - rect.Height / 2.0) * (circleDistance.Y - rect.Height / 2.0);

                        if (cornerDistance_sq <= (circleRad * circleRad))
                        {
                            collided = true;
                        }
                    }
                }

            }

            if (collided)
            {
                v = circle - rectCenter;
            }

            return v;
        }

        private Vector2 CollisionVectorBoxImage(Collidable that)
        {
            if (that.HasBoxCollidableShape)
            {
                Vector2 v = CollisionVectorBoxBox(that);
                return v;
            }
            return Vector2.Zero;
        }
    }
}
