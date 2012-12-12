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
                                    tileSet = new TileImage(game, xmlReader);
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
