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
    public class StaticSprite : Sprite
    {
        private Texture2D image = null;

        public StaticSprite(string imageName)
        {
            image = SpiridiGame.imageManager.GetImage(imageName);
        }

        // TODO: most of these parameters should be PROPERTIES of the sprite, not parameters to the draw method.
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 centerOffset, Color tintColor, float rotation, float layer)
        {
            spriteBatch.Draw(this.image, position + centerOffset, null, tintColor, rotation, centerOffset, 1.0f, SpriteEffects.None, layer);
        }

        public int Width
        {
            get { return image.Width; }
        }

        public int Height
        {
            get { return image.Height; }
        }

        public void Update(TimeSpan elapsedTime)
        {
        }
    }
}
