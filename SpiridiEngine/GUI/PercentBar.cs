/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine.GUI
{
    public class PercentBar : Window
    {
        // TODO: this needs to be Image after refactor.
        private Texture2D barImage = null;
        private double percent;
        private Rectangle destinationRectangle;


        public PercentBar(String barImage)
        {
            this.barImage = SpiridiGame.ImageManagerInstance.GetImage(barImage);
            Percent = 1.0;
        }

        public double Percent
        { 
            get { return percent; }
            set
            {
                percent = value;

                // TODO: change this to be chosen by user.
                destinationRectangle.Width = (int)(barImage.Width * percent);
                destinationRectangle.Height = (int)(barImage.Height /* * percent*/ );
            }
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                destinationRectangle.X = (int)value.X;
                destinationRectangle.Y = (int)value.Y;
                base.Position = value;
            }
        }

        public override int Height
        {
            get { return barImage.Height; }
        }

        public override int Width
        {
            get { return barImage.Width; }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(this.barImage, destinationRectangle, Color.White);

        }
    }
}
