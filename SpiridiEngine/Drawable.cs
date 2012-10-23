using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Interface for anything that's drawable
    /// </summary>
    public interface Drawable
    {
        void Draw(SpriteBatch spriteBatch);
    }
}
