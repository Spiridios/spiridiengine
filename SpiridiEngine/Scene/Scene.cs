/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class Scene : Drawable, Updatable
    {
        private SpiridiGame game;

        private List<SceneLayer> layers = new List<SceneLayer>();
        private Dictionary<string, SceneLayer> layerNameMap = new Dictionary<string, SceneLayer>();

        public Scene(SpiridiGame game) 
        {
            this.game = game;
        }

        public void LoadTiledMap(string tiledFile)
        {
            List<SceneLayer> tmpLayers = TileMapLayer.LoadTiledMap(game, tiledFile);
            layers.AddRange(tmpLayers);
            foreach(SceneLayer layer in tmpLayers)
            {
                if (layer.Name != null)
                {
                    layerNameMap.Add(layer.Name, layer);
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

        public SceneLayer GetLayer(String layerName)
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
        }
    }
}
