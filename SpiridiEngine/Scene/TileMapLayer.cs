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
using System.IO.Compression;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    class TileMapLayer : SceneLayer
    {
        private const string TILED_ROOT_ELEMENT = "map";
        private const string TILED_LAYER_ELEMENT = "layer";

        private SpiridiGame game;
        private List<int> layerTileIndices = null;
        private TileImage tileSet;
        private int layerWidth;
        private int layerHeight;

        public TileMapLayer(SpiridiGame game, TileImage tileSet, XmlReader mapLayerReader)
        {
            this.game = game;
            LoadTiledLayer(mapLayerReader);
            this.tileSet = tileSet;
        }

        public static List<SceneLayer> LoadTiledMap(SpiridiGame game, string tiledFile)
        {
            TileImage tileSet = null;
            List<SceneLayer> layers = new List<SceneLayer>();
            using (FileStream fileStream = new FileStream(tiledFile, FileMode.Open))
            {
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch (xmlReader.Name)
                            {
                                case (TileMapLayer.TILED_ROOT_ELEMENT):
                                    break;
                                case (TileImage.TILED_TILESET_ELEMENT):
                                    tileSet = new TileImage(xmlReader);
                                    break;
                                case (TileMapLayer.TILED_LAYER_ELEMENT):
                                    layers.Add(new TileMapLayer(game, tileSet, xmlReader));
                                    break;
                                case ("objectgroup"):
                                    break; //ignore it for now.
                                default:
                                    throw new InvalidDataException(String.Format("Unsupported tag '{0}'", xmlReader.Name));
                            }
                        }
                    }
                }
            }
            return layers;
        }

        private void LoadTiledLayer(XmlReader xmlReader)
        {
            do
            {
                switch (xmlReader.NodeType)
                {
                    case (XmlNodeType.Element):
                        switch (xmlReader.Name)
                        {
                            case(TileMapLayer.TILED_LAYER_ELEMENT):
                                layerHeight = int.Parse(xmlReader.GetAttribute("height"));
                                layerWidth = int.Parse(xmlReader.GetAttribute("width"));
                                break;
                            case ("data"):
                                string encoding = xmlReader.GetAttribute("encoding");
                                if (encoding != "base64")
                                {
                                    throw new InvalidDataException(String.Format("Unsupported encoding {0}",encoding));
                                }

                                string compression = xmlReader.GetAttribute("compression");
                                if(!String.IsNullOrEmpty(compression) && compression != "gzip")
                                {
                                    throw new InvalidDataException(String.Format("Unsupported compression {0}",compression));
                                }

                                string layerString = xmlReader.ReadElementContentAsString().Trim();
                                byte[] rawLayer = Convert.FromBase64String(layerString);

                                int size = layerWidth * layerHeight;
                                layerTileIndices = new List<int>(size);
                                
                                Stream layerStream = new MemoryStream(rawLayer);
                                if(compression == "gzip")
                                {
                                    layerStream = new GZipStream(layerStream, CompressionMode.Decompress);
                                }

                                using (BinaryReader layerReader = new BinaryReader(layerStream))
                                {
                                    for (int i = 0; i < size; i++)
                                    {
                                        layerTileIndices.Add(layerReader.ReadInt32());
                                    }
                                }
                                break;
                            default:
                                throw new InvalidOperationException(string.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == TILED_LAYER_ELEMENT)
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(tileSet, spriteBatch);
        }

        private void Draw(TileImage tileSet, SpriteBatch spriteBatch)
        {
            // TODO: Possibly call from update instead of draw
            actors.Sort(actorsComparer);

            int size = layerTileIndices.Count;
            int currentActorIndex = 0;
            Actor currentActor = (currentActorIndex < actors.Count) ? actors[currentActorIndex] : null;

            for (int i = 0; i < size; i++)
            {
                Vector2 destCoord = TileImage.GetImageCoordinatesFromOffset(i, layerWidth, tileSet.TileWidth, tileSet.TileHeight);
                if (currentActor != null && currentActor.Position.Y < destCoord.Y)
                {
                    currentActor.Draw(spriteBatch);
                    currentActorIndex++;
                    currentActor = (currentActorIndex < actors.Count) ? actors[currentActorIndex] : null;
                }

                int gid = layerTileIndices[i];
                if (gid > 0)
                {
                    Rectangle dest = new Rectangle((int)destCoord.X, (int)destCoord.Y, tileSet.TileWidth, tileSet.TileHeight);
                    tileSet.DrawTile(spriteBatch, gid, dest);
                }
            }
        }
    }
}
