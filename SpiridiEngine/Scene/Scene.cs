/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine.Scene
{
    public class Scene : Drawable, Updatable
    {
        private SpiridiGame game;

        private Camera camera;
        private List<SceneLayer> layers = new List<SceneLayer>();
        private Dictionary<string, SceneLayer> layerNameMap = new Dictionary<string, SceneLayer>();

        public Scene(SpiridiGame game) 
        {
            this.camera = new Camera();
            this.game = game;
        }

        public Camera Camera
        {
            get { return this.camera; }
            set
            {
                this.camera = value;
                foreach(SceneLayer layer in layers)
                {
                    foreach (Actor actor in layer.Actors)
                    {
                        actor.Camera = value;
                    }
                }
            }
        }

        public void LoadTiledMap(string tiledFile)
        {
            List<TileMapLayer> tmpLayers = TileMapLayer.LoadTiledMap(game, this, tiledFile);
            layers.AddRange(tmpLayers);

            // Create name map
            foreach (TileMapLayer layer in tmpLayers)
            {
                if (layer.HasName)
                {
                    layerNameMap.Add(layer.Name, layer);
                }
            }

            // Find collision layer(s) and add them to the collision detectors.
            foreach (TileMapLayer layer in tmpLayers)
            {
                if (layer.HasCollisionLayer)
                {
                    layer.CollisionDetector.AddMapCollisionLayer((TileMapLayer)GetLayer(layer.CollisionLayerName));
                }
            }
        }

        public void AddLayer(SceneLayer layer)
        {
            this.layers.Add(layer);
        }

        public void AddLayer(SceneLayer layer, string layerName)
        {
            this.layerNameMap.Add(layerName, layer);
            AddLayer(layer);
        }

        public void AddLayer(SceneLayer layer, int layerIndex)
        {
            this.layers.Insert(layerIndex, layer);
        }

        public void AddLayer(SceneLayer layer, int layerIndex, string layerName)
        {
            this.layerNameMap.Add(layerName, layer);
            AddLayer(layer, layerIndex);
        }

        public SceneLayer GetLayer(int layerIndex)
        {
            return this.layers[layerIndex];
        }

        public SceneLayer GetLayer(string layerName)
        {
            SceneLayer sceneLayer = null;
            layerNameMap.TryGetValue(layerName, out sceneLayer);
            return sceneLayer;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }

        public void Update(System.TimeSpan elapsedTime)
        {
            foreach (SceneLayer layer in layers)
            {
                layer.Update(elapsedTime);
            }
            ProcessCollisions();
        }

        public void ProcessCollisions()
        {
            foreach (SceneLayer layer in layers)
            {
                layer.ProcessCollisions();
            }
        }
    }
}
