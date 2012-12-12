using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Spiridios.SpiridiEngine
{
    class TileMapLayer
    {
        public const string TILED_ELEMENT = "layer";
        private SpiridiGame game;
        private List<int> layerTileIndices = null;
        private int layerWidth;
        private int layerHeight;

        public TileMapLayer(SpiridiGame game, XmlReader mapLayerReader)
        {
            this.game = game;
            LoadTiledLayer(mapLayerReader);
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
                            case(TileMapLayer.TILED_ELEMENT):
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
                        if (xmlReader.Name == TILED_ELEMENT)
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }

        public void Draw(TileImage tileSet, SpriteBatch spriteBatch)
        {
            int size = layerTileIndices.Count;
            for (int i = 0; i < size; i++)
            {
                int gid = layerTileIndices[i];
                if (gid > 0)
                {
                    Vector2 destCoord = TileImage.GetImageCoordinatesFromOffset(i, layerWidth, tileSet.TileWidth, tileSet.TileHeight);
                    Rectangle dest = new Rectangle((int)destCoord.X, (int)destCoord.Y, tileSet.TileWidth, tileSet.TileHeight);
                    tileSet.DrawTile(spriteBatch, gid, dest);
                }
            }
        }
    }
}
