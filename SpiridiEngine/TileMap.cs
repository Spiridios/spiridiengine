using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;

namespace Spiridios.SpiridiEngine
{
    public class TileMap : Drawable
    {
        private SpiridiGame game;
        private Texture2D tileSet = null;
        private List<int> mapGIDs = null;

        public TileMap(SpiridiGame game, string tiledFile) 
        {
            this.game = game;
            LoadTiledMap(tiledFile);
        }

        private void LoadTiledMap(string tiledFile)
        {
            XDocument tiledDoc = XDocument.Load(tiledFile);
            XElement tileSetElement = tiledDoc.Root.Element("tileset");
            XElement tileSetImage = tileSetElement.Element("image");
            string tileSetImageSource = tileSetImage.Attribute("source").Value;
            tileSet = game.ImageManager.AddImage(tileSetImageSource, tileSetImageSource);

            XElement layerElement = tiledDoc.Root.Element("layer");
            int layerHeight = int.Parse(layerElement.Attribute("height").Value);
            int layerWidth = int.Parse(layerElement.Attribute("width").Value);
            string layerString = layerElement.Element("data").Value;
            byte[] rawLayer = Convert.FromBase64String(layerString);
            int size = layerWidth * layerHeight; 
            mapGIDs = new List<int>(size);
            using (BinaryReader stuff = new BinaryReader(new GZipStream(new MemoryStream(rawLayer), CompressionMode.Decompress)))
            {

                for (int i = 0; i < size; i++)
                {
                    mapGIDs.Add(stuff.ReadInt32());
                }
            }
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.tileSet, new Vector2(0, 0), Color.White);
        }

    }
}
