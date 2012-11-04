using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TileMap : Drawable
    {
        private SpiridiGame game;

        // Need multiples of these.
        private TileImage tileSet = null;

        private List<int> layerTileIndices = null;
        private Vector2 layerSize = new Vector2();

        public TileMap(SpiridiGame game, string tiledFile) 
        {
            this.game = game;
            LoadTiledMap(tiledFile);
        }

        private void LoadTiledMap(string tiledFile)
        {
            XDocument tiledDoc = XDocument.Load(tiledFile);
            tileSet = new TileImage(game, tiledDoc.Root.Element("tileset"));

            XElement layerElement = tiledDoc.Root.Element("layer");
            layerSize.Y = int.Parse(layerElement.Attribute("height").Value);
            layerSize.X = int.Parse(layerElement.Attribute("width").Value);
            string layerString = layerElement.Element("data").Value;
            byte[] rawLayer = Convert.FromBase64String(layerString);
            int size = (int)(layerSize.X * layerSize.Y); 
            layerTileIndices = new List<int>(size);
            using (BinaryReader layerReader = new BinaryReader(new GZipStream(new MemoryStream(rawLayer), CompressionMode.Decompress)))
            {
                for (int i = 0; i < size; i++)
                {
                    layerTileIndices.Add(layerReader.ReadInt32());
                }
            }
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int size = (int)(layerSize.X * layerSize.Y);
            for (int i = 0; i < size; i++)
            {
                int gid = layerTileIndices[i];
                if (gid > 0)
                {
                    Vector2 destCoord = TileImage.GetImageCoordinatesFromOffset(i, (int)layerSize.X, tileSet.TileWidth, tileSet.TileHeight);
                    Rectangle dest = new Rectangle((int)destCoord.X, (int)destCoord.Y, tileSet.TileWidth, tileSet.TileHeight);
                    tileSet.DrawTile(spriteBatch, gid, dest);
                }
            }
        }

    }
}
