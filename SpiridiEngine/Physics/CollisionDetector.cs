/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Spiridios.SpiridiEngine.Scene;

namespace Spiridios.SpiridiEngine.Physics
{
    /// <summary>
    /// Detects, but does not resolve, collisions.
    /// </summary>
    public class CollisionDetector
    {
        private TileMapLayer mapCollisionLayer;
        private List<Collidable> collidables = new List<Collidable>();

        public void AddMapCollisionLayer(TileMapLayer tileMapLayer)
        {
            mapCollisionLayer = tileMapLayer;
        }

        public void AddCollidable(Collidable collidable)
        {
            this.collidables.Add(collidable);
        }

        public void ProcessCollisions()
        {
            foreach (Collidable collidable in collidables)
            {
                List<Collidable> activeCollidables = new List<Collidable>();

                foreach (Collidable otherCollidable in collidables)
                {
                    if (otherCollidable != collidable)
                    {
                        if (collidable.CollidesWith(otherCollidable))
                        {
                            activeCollidables.Add(otherCollidable);
                        }
                    }
                }

                if (mapCollisionLayer != null)
                {
                    Rectangle actorBounds = collidable.BoundingBox;
                    List<Collidable> tileCollidables = mapCollisionLayer.GetCollidables(actorBounds);
                    foreach (Collidable tileCollidable in tileCollidables)
                    {
                        if (collidable.CollidesWith(tileCollidable))
                        {
                            activeCollidables.Add(tileCollidable);
                        }
                    }
                }

                if (activeCollidables.Any())
                {
                    collidable.NotifyCollisionListeners(activeCollidables);
                }
            }
        }

    }
}
