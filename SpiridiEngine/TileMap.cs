using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Spiridios.SpiridiEngine
{
    public class TileMap : Drawable
    {
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
                XmlReader crap = XmlReader.Create(fileStream);
                XmlDocument tiledDoc = new XmlDocument();
                tiledDoc.Load(crap);
                tileSet = new TileImage(game, tiledDoc.DocumentElement.SelectSingleNode("tileset"));

                foreach (XmlNode layerElement in tiledDoc.DocumentElement.SelectNodes("layer"))
                {
                    layers.Add(new TileMapLayer(game, layerElement));
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
