using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    class StaticSprite : Sprite
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
