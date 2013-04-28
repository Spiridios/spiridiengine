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
        private String tag = null;

        // TODO: this shouldn't be needed anymore.
        private bool confineNormals = false;

        // TODO: this shouldn't be needed anymore.
        public bool OrthoVectors
        {
            get { return confineNormals; }
            set { confineNormals = value; }
        }

        public String Tag
        {
            get { return this.tag; }
            set { this.tag = value; }
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
            Vector2 v = Vector2.Zero;
            Rectangle thisBoundingBox = this.BoundingBox;
            Rectangle thatBoundingBox = that.BoundingBox;
            Rectangle intersection = Rectangle.Intersect(thisBoundingBox, thatBoundingBox);

            if (intersection.Width > 0 && intersection.Height > 0)
            {
                // Translate into local coordinates.
                intersection.X = intersection.X - thatBoundingBox.X;
                intersection.Y = intersection.Y - thatBoundingBox.Y;

                // TODO: This was originally a set to remove dupes.
                List<Point> points = GetRectangleIntersections(that.imageShape.Image, intersection);
                if (points.Count == 1)
                {
                    System.Diagnostics.Debug.Print("Only one collision point found!!! Fix this somehow!");
                }
                else if (points.Count > 1)
                {
                    v = this.GetNormalFromPoints(that.imageShape.Image, points[0], points[1]);
                }
            }
            return v;
        }

        // Maybe this belongs in image....
        private List<Point> GetRectangleIntersections(Image image, Rectangle screenRect)
        {
            // JSIL can't iterate over a HashSet, so build parallel containers.
            HashSet<Point> pointSet = new HashSet<Point>();
            List<Point> points = new List<Point>();
            Point tempPt;

            // Upper Left
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Left, screenRect.Top), 1, 0, screenRect.Width);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Left, screenRect.Top), 0, 1, screenRect.Height);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            // Lower Left
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Left, screenRect.Bottom-1), 1, 0, screenRect.Width);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Left, screenRect.Bottom-1), 0, -1, screenRect.Height);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            // Lower Right
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Right-1, screenRect.Bottom-1), -1, 0, screenRect.Width);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Right-1, screenRect.Bottom-1), 0, -1, screenRect.Height);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            // Upper Right
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Right-1, screenRect.Top), -1, 0, screenRect.Width);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }
            tempPt = this.GetLineIntersection(image, new Point(screenRect.Right-1, screenRect.Top), 0, 1, screenRect.Height);
            if (tempPt.X >= 0 && tempPt.Y >= 0 && !pointSet.Contains(tempPt))
            {
                pointSet.Add(tempPt);
                points.Add(tempPt);
            }

            return points;
        }

        /// <summary>
        /// Used to scan a horizontal or vertical line to find where it intersects with the contained image.
        /// The pixel of intersection is always colored.
        /// </summary>
        /// <param name="x">The starting coordinate</param>
        /// <param name="y">The starting coordinate</param>
        /// <param name="xIncrement">Number to add to X coordinate in each step</param>
        /// <param name="yIncrement">Number to add to Y coordinate in each step</param>
        /// <param name="numSteps">Number of steps to execute</param>
        /// <returns>The point of intersection. If either X or Y (or both) are less than zero, then no intersection was found.</returns>
        private Point GetLineIntersection(Image image, Point position, int xIncrement, int yIncrement, int numSteps, byte alphaCutoff = 10)
        {
            int x = position.X;
            int y = position.Y;
            Point intersection = new Point(-1, -1);

            Color pixel = image.GetPixel(x, y);
            // True == pixel, false == no pixel
            bool startState = pixel.A > alphaCutoff;

            for (int i = 1; i < numSteps; i++)
            {
                x += xIncrement;
                y += yIncrement;
                pixel = image.GetPixel(x, y);
                if (startState != (pixel.A > alphaCutoff))
                {
                    // Pixel If we started on a pixel, then the pixel we're on is empty,
                    // and we need to get the previous pixel as our intersection point
                    intersection.X = startState ? x - xIncrement : x;
                    intersection.Y = startState ? y - yIncrement : y;
                    // Exit loop 
                    break;
                }
            }

            if(startState && (intersection.X < 0 || intersection.Y < 0))
            {
                // Means we were ENTIRELY within something. Use starting point as point of intersection
                intersection = position;
            }

            return intersection;
        }

        private Vector2 GetNormalFromPoints(Image image, Point point1, Point point2, byte alphaCutoff = 10)
        {
            Vector2 normal = new Vector2(-(point1.Y - point2.Y), (point1.X - point2.X));
            if (normal != Vector2.Zero)
            {
                normal.Normalize();
                Vector2 test = normal * 2;
                int testX = point1.X + (int)test.X;
                int testY = point1.Y + (int)test.Y;

                Color color = image.GetPixel(testX, testY);
                if (color.A > alphaCutoff)
                {
                    normal = -normal;
                }
            }
            return normal;
        }

    }
}
