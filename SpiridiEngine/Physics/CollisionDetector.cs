using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
