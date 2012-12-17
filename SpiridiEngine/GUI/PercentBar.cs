using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine.GUI
{
    public class PercentBar : Window
    {
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
