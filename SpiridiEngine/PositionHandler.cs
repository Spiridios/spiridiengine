using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Wraps a position, allowing alternative implementations.
    /// </summary>
    public class PositionHandler
    {
        public virtual Vector2 Position { get; set; }
    }
}
