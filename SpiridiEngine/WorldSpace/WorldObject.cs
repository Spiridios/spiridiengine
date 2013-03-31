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

namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Represents an object in 2d space
    /// </summary>
    public abstract class WorldObject : Drawable, Updatable
    {
        private ScreenObject screenObject;

        public WorldObject()
        {
            this.PositionHandler = new PositionHandler();
        }

        public WorldObject(PositionHandler positionHandler)
        {
            this.PositionHandler = positionHandler;
        }

        protected PositionHandler PositionHandler { get; set; }

        protected void AddScreenObject(ScreenObject screenObject)
        {
            this.screenObject = screenObject;
            screenObject.Position = PositionHandler.Position;
        }

        // TODO: make this non-virtual. Note: if this becomes non-virtual, then there NEEDS to be a
        // PositionUpdated event so child objects can detect a change and react to it.
        public virtual Vector2 Position
        {
            get { return this.PositionHandler.Position; }
            set
            {
                this.PositionHandler.Position = value;
                if (screenObject != null)
                {
                    screenObject.Position = this.PositionHandler.ScreenPosition;
                }
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(TimeSpan elapsedTime);
    }
}
