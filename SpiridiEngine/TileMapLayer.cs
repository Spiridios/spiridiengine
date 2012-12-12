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
        private SpiridiGame game;
        private List<int> layerTileIndices = null;
        private int layerWidth;
        private int layerHeight;

        public TileMapLayer(SpiridiGame game, XmlNode layerElement)
        {
            this.game = game;
            LoadTiledLayer(layerElement);
        }

        private void LoadTiledLayer(XmlNode layerElement)
        {
            layerHeight = int.Parse(layerElement.Attributes["height"].Value);
            layerWidth = int.Parse(layerElement.Attributes["width"].Value);

            XmlNode dataNode = layerElement.SelectSingleNode("data");

            string layerString = dataNode.FirstChild.Value;

            byte[] rawLayer = Convert.FromBase64String(layerString);
            int size = layerWidth * layerHeight;
            layerTileIndices = new List<int>(size);
            using (BinaryReader layerReader = new BinaryReader(new GZipStream(new MemoryStream(rawLayer), CompressionMode.Decompress)))
            {
                for (int i = 0; i < size; i++)
                {
                    layerTileIndices.Add(layerReader.ReadInt32());
                }
            }
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
