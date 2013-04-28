/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using Microsoft.Xna.Framework;
using Spiridios.SpiridiEngine.Input;

namespace Spiridios.SpiridiEngine
{
    public class State
    {
        // TODO: could this be genericized so states always have native access to their type?
        protected SpiridiGame game;
        protected InputManager inputManager;

        public State(SpiridiGame game)
        {
            this.game = game;
            this.inputManager = game.InputManager;
        }

        public virtual void Initialize()
        {
            if (inputManager == null)
            {
                this.inputManager = game.InputManager;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        // TODO: this needs to take a spriteBatch, you should NOT be forced to fetch it from Game.
        public virtual void Draw(GameTime gameTime)
        {
        }


        public virtual void OnClick()
        {
        }

        public virtual void KeyUp(KeyboardEvent keyState)
        {
        }

        public virtual void KeyDown(KeyboardEvent keyState)
        {
        }
    }
}
