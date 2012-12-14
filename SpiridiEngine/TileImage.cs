using System;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    class TileImage
    {
        public const string TILED_ELEMENT = "tileset";
        private SpiridiGame game;
        private Texture2D tileSet = null;
        private int tileWidth = 0;
        private int tileHeight = 0;

        public int TileWidth
        {
            get { return tileWidth; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
        }

        public TileImage(SpiridiGame game, string imageName, int tileWidth, int tileHeight)
        {
            tileSet = game.ImageManager.GetImage(imageName);
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }

        public TileImage(SpiridiGame game, XmlReader tileSetReader)
        {
            this.game = game;
            LoadFromTiledElement(tileSetReader);
        }

        private void LoadFromTiledElement(XmlReader xmlReader)
        {
            do
            {
                switch (xmlReader.NodeType)
                {
                    case (XmlNodeType.Element):
                        switch (xmlReader.Name)
                        {
                            case (TileImage.TILED_ELEMENT):
                                tileWidth = int.Parse(xmlReader.GetAttribute("tilewidth"));
                                tileHeight = int.Parse(xmlReader.GetAttribute("tileheight"));
                                break;
                            case ("image"):
                                string tileSetImageSource = xmlReader.GetAttribute("source");
                                tileSet = this.game.ImageManager.AddImage(tileSetImageSource, tileSetImageSource);
                                //tileSheetSize.X = int.Parse(tileSetImage.GetAttribute("width"));
                                //tileSheetSize.Y = int.Parse(tileSetImage.GetAttribute("height"));
                                break;
                            default:
                                throw new InvalidDataException(String.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
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

        // Move to Vector2Ext maybe
        public static Vector2 GetImageCoordinatesFromOffset(int index, int imageWidth, int xMult, int yMult)
        {
            int y = index / imageWidth;
            int x = index % imageWidth;
            Vector2 coords = new Vector2(x * xMult, y * yMult);
            return coords;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="tileId">The ID of the tile to draw. 0 means don't draw the tile, 1 is the upper-leftmost tile, 2 is the tile to the right of that tile.</param>
        public void DrawTile(SpriteBatch spriteBatch, int tileId, Rectangle destination)
        {
            if (tileId > 0)
            {
                Vector2 srcCoord = TileImage.GetImageCoordinatesFromOffset(tileId - 1, (tileSet.Width / tileWidth), tileWidth, tileHeight);
                Rectangle source = new Rectangle((int)srcCoord.X, (int)srcCoord.Y, tileWidth, tileHeight);

                spriteBatch.Draw(this.tileSet, destination, source, Color.White);
            }

        }

    }
}
