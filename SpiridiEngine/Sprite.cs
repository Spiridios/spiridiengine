using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public interface Sprite : Updatable
    {
        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 centerOffset, Color tintColor, float rotation, float layer);

    }
}
