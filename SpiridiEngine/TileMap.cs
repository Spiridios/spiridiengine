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
    public class TileMap : Drawable
    {
        public const string TILED_ELEMENT = "map";
        private SpiridiGame game;

        // Need multiples of these.
        private TileImage tileSet = null;

        private List<TileMapLayer> layers = new List<TileMapLayer>();

        public TileMap(SpiridiGame game, string tiledFile) 
        {
            this.game = game;
            LoadTiledMap(tiledFile);
        }

        private void LoadTiledMap(string tiledFile)
        {
            using (FileStream fileStream = new FileStream(tiledFile, FileMode.Open))
            {
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch(xmlReader.Name)
                            {
                                case(TileMap.TILED_ELEMENT):
                                    break;
                                case(TileImage.TILED_ELEMENT):
                                    tileSet = new TileImage(xmlReader);
                                    break;
                                case(TileMapLayer.TILED_ELEMENT):
                                    layers.Add(new TileMapLayer(game, xmlReader));
                                    break;
                                case("objectgroup"):
                                    break; //ignore it for now.
                                default:
                                    throw new InvalidDataException(String.Format("Unsupported tag '{0}'", xmlReader.Name));
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileMapLayer layer in layers)
            {
                layer.Draw(tileSet, spriteBatch);
            }
        }

    }
}
