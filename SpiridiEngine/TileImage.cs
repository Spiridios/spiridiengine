using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Spiridios.SpiridiEngine
{
    class TileImage
    {
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

        public TileImage(SpiridiGame game, XmlNode tileSetElement)
        {
            this.game = game;
            LoadFromTiledElement(tileSetElement);
        }

        private void LoadFromTiledElement(XmlNode tileSetElement)
        {
            XmlNode tileSetImage = tileSetElement.SelectSingleNode("image");
            string tileSetImageSource = tileSetImage.Attributes["source"].Value;
            tileWidth = int.Parse(tileSetElement.Attributes["tilewidth"].Value);
            tileHeight = int.Parse(tileSetElement.Attributes["tileheight"].Value);
            tileSet = this.game.ImageManager.AddImage(tileSetImageSource, tileSetImageSource);
            //tileSheetSize.X = int.Parse(tileSetImage.Attributes["width"].Value);
            //tileSheetSize.Y = int.Parse(tileSetImage.Attributes["height"].Value);
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
