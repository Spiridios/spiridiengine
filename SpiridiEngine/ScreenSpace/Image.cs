using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class Image : ScreenObject
    {
        private Sprite image;

        public Image(string imageName)
            :this(new StaticSprite(imageName))
        {

        }

        public Image(Sprite image)
        {
            this.image = image;
        }

        public Vector2 CenterOffset
        {
            get { return this.image.CenterOffset; }
        }

        public int Width
        {
            get { return this.image.Width; }
        }

        public int Height
        {
            get { return this.image.Height; }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.image.Draw(spriteBatch, this.Position);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            this.image.Update(elapsedTime);
        }
    }
}
