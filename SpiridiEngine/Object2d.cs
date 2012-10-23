using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Represents an object in 2d space
    /// </summary>
    public abstract class Object2d : Drawable, Updatable
    {
        public Object2d()
        {
            this.PositionHandler = new PositionHandler();
        }

        protected PositionHandler PositionHandler { get; set; }

        // TODO: make this non-virtual
        public virtual Vector2 Position
        {
            get { return this.PositionHandler.Position; }
            set { this.PositionHandler.Position = value; }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(TimeSpan elapsedTime);
    }
}
