/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;

namespace Spiridios.SpiridiEngine.GUI
{
    public class ImageButton : Window
    {
        private AnimatedSprite buttonSprite;

        public ImageButton(AnimatedSprite buttonSprite, int upFrame, int downFrame)
        {
            this.buttonSprite = buttonSprite;
            buttonSprite.CurrentFrame = upFrame;
        }

        public override int Height
        {
            get { return buttonSprite.Height; }
        }

        public override int Width
        {
            get { return buttonSprite.Width; }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            // swallow it for now.
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            buttonSprite.Draw(spriteBatch, this.Position);
        }

    }
}
