/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class AnimatedSprite : Sprite
    {
        private TileImage image = null;
        private int currentFrameIndex = 0;

        public AnimatedSprite(string imageName, int tileWidth, int tileHeight)
            : base()
        {
            image = new TileImage(imageName, tileWidth, tileHeight);
        }

        // TODO: most of these parameters should be PROPERTIES of the sprite, not parameters to the draw method.
        public override void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 centerOffset)
        {
            Rectangle source = this.image.GetTileSourceRect(currentFrameIndex);
            spriteBatch.Draw(this.image.Image, position + centerOffset, source, TintColor, Rotation, centerOffset, 1.0f, SpriteEffects.None, Layer);
        }

        public override int Width
        {
            get { return image.TileWidth; }
        }

        public override int Height
        {
            get { return image.TileHeight; }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            // TODO: switch current frame index.
        }
    }
}
