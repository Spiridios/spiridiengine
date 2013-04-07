/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
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
        private List<Image> layerTileImages = null;
        private TileSetCollection tileSet;
        private int layerWidth;
        private int layerHeight;
        private int tileWidth;
        private int tileHeight;

        public TileMapLayer(SpiridiGame game, TileSetCollection tileSet, XmlReader mapLayerReader)
        {
            this.game = game;
            this.tileSet = tileSet;
            LoadTiledLayer(mapLayerReader);
        }

        public static List<SceneLayer> LoadTiledMap(SpiridiGame game, string tiledFile)
        {
            TileSet tileSet = null;
            TileSetCollection tileImageSet = new TileSetCollection();
            int tileWidth = 0;
            int tileHeight = 0;

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
                                    tileWidth = int.Parse(xmlReader.GetAttribute("tilewidth"));
                                    tileHeight = int.Parse(xmlReader.GetAttribute("tileheight"));
                                    break;
                                case (TileSet.TILED_TILESET_ELEMENT):
                                    //string tilesetName = TileSet.ParseTiledTilesetName(xmlReader);
                                    int startTileId = int.Parse(xmlReader.GetAttribute("firstgid"));
                                    tileSet = new TileSet(xmlReader);
                                    tileImageSet.AddTileSet(tileSet, startTileId);
                                    break;
                                case (TileMapLayer.TILED_LAYER_ELEMENT):
                                    TileMapLayer mapLayer = new TileMapLayer(game, tileImageSet, xmlReader);
                                    mapLayer.tileWidth = tileWidth;
                                    mapLayer.tileHeight = tileHeight;
                                    layers.Add(mapLayer);
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
                                this.Name = xmlReader.GetAttribute("name");
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
                                layerTileImages= new List<Image>(size);
                                
                                Stream layerStream = new MemoryStream(rawLayer);
                                if(compression == "gzip")
                                {
                                    layerStream = new GZipStream(layerStream, CompressionMode.Decompress);
                                }

                                using (BinaryReader layerReader = new BinaryReader(layerStream))
                                {
                                    for (int i = 0; i < size; i++)
                                    {
                                        int tileId = layerReader.ReadInt32();
                                        layerTileImages.Add(this.tileSet.GetImage(tileId));
                                    }
                                }
                                break;
                            case ("properties"):
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

        private void Draw(TileSetCollection tileSet, SpriteBatch spriteBatch)
        {
            // TODO: Possibly call from update instead of draw
            actors.Sort(actorsComparer);

            int size = layerTileImages.Count;
            int currentActorIndex = 0;
            Actor currentActor = (currentActorIndex < actors.Count) ? actors[currentActorIndex] : null;

            for (int i = 0; i < size; i++)
            {
                Vector2 destCoord = TileSet.GetImageCoordinatesFromOffset(i, layerWidth, tileWidth, tileHeight);
                if (currentActor != null && (currentActor.Position.Y + currentActor.Height) < (destCoord.Y + tileHeight))
                {
                    currentActor.Draw(spriteBatch);
                    currentActorIndex++;
                    currentActor = (currentActorIndex < actors.Count) ? actors[currentActorIndex] : null;
                }

                Image image = layerTileImages[i];
                if (image != null)
                {
                    image.Draw(spriteBatch, destCoord);
                }
            }

            // Draw any  undrawn actors.
            for (int i = currentActorIndex; i < actors.Count; i++)
            {
                actors[currentActorIndex].Draw(spriteBatch);
            }
        }
    }
}
