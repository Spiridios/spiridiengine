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

        private TileMapLayer layer;

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
            layer = new TileMapLayer(game, layerElement);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            layer.Draw(tileSet, spriteBatch);
        }

    }
}
