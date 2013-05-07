﻿/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public abstract class TileMapImage : Image
    {
        private List<Image> layerTileImages = null;
        private int imageWidth;
        private int imageHeight;
        private int tileWidth;
        private int tileHeight;

        public TileMapImage()
            : base()
        {
        }

        public override int Width
        {
            get { return imageWidth; }
        }

        public override int Height
        {
            get { return imageHeight; }
        }

        protected override void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            //spriteBatch.Draw(this.image, destination, sourceRect, tintColor, rotation, Origin, SpriteEffects.None, layer);
        }

    }
}