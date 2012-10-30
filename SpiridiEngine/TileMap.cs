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
            string layerString = tiledDoc.Root.Element("layer").Element("data").Value;
            byte[] rawLayer = Convert.FromBase64String(layerString);
            int size = rawLayer.Length / sizeof(Int32); // TODO: this is not the size of the layer, it's the size of the COMPRESSED data.
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
