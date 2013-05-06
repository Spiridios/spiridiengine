/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class TileSet
    {
        internal const string TILED_TILESET_ELEMENT = "tileset";
        private const string TILED_TILESET_IMAGE_ELEMENT = "image";

        // TODO: shouldn't this be an image?
        private Texture2D tileSet = null;
        private int tileWidth = 0;
        private int tileHeight = 0;
        private int tileCount = 0;

        public TileSet(string imageName, int tileWidth, int tileHeight)
        {
            tileSet = SpiridiGame.ImageManagerInstance.GetImage(imageName);
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            ComputeTileCount();
        }

        public TileSet(XmlReader tileSetReader)
        {
            LoadFromTiledElement(tileSetReader);
            ComputeTileCount();
        }

        public int TileWidth
        {
            get { return tileWidth; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
        }

        public int TileCount
        {
            get { return tileCount; }
        }

        private void ComputeTileCount()
        {
            tileCount = (tileSet.Width / tileWidth) * (tileSet.Height / tileHeight);
        }

        private static string ParseTiledTilesetName(XmlReader xmlReader)
        {
            string tilesetName = null;
            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == TileSet.TILED_TILESET_ELEMENT)
            {
                tilesetName = xmlReader.GetAttribute("name");
            }
            return tilesetName;
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
                            case (TileSet.TILED_TILESET_ELEMENT):
                                tileWidth = int.Parse(xmlReader.GetAttribute("tilewidth"));
                                tileHeight = int.Parse(xmlReader.GetAttribute("tileheight"));
                                break;
                            case (TileSet.TILED_TILESET_IMAGE_ELEMENT):
                                string tileSetImageSource = xmlReader.GetAttribute("source");
                                tileSet = SpiridiGame.ImageManagerInstance.AddImage(tileSetImageSource, tileSetImageSource);
                                //tileSheetSize.X = int.Parse(tileSetImage.GetAttribute("width"));
                                //tileSheetSize.Y = int.Parse(tileSetImage.GetAttribute("height"));
                                break;
                            default:
                                throw new InvalidDataException(String.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == TileSet.TILED_TILESET_ELEMENT)
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
                Rectangle source = GetTileSourceRect(tileId);
                spriteBatch.Draw(this.tileSet, destination, source, Color.White);
            }
        }

        public Texture2D Texture
        {
            get { return this.tileSet; }
        }

        // TODO: this does not cache the Image
        public Image CreateTileImage(int tileId)
        {
            Rectangle source = GetTileSourceRect(tileId);
            // TODO: remove me
            Image tileImage = new SubsetImage(new TextureImage(this.tileSet), source);
            return tileImage;
        }

        public Rectangle GetTileSourceRect(int tileId)
        {
            Vector2 srcCoord = TileSet.GetImageCoordinatesFromOffset(tileId - 1, (tileSet.Width / tileWidth), tileWidth, tileHeight);
            Rectangle source = new Rectangle((int)srcCoord.X, (int)srcCoord.Y, tileWidth, tileHeight);
            return source;
        }

    }
}

