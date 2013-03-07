/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class State
    {
        protected SpiridiGame game;

        public State(SpiridiGame game)
        {
            this.game = game;
        }

        // TODO: have this automagically called when states change?
        public virtual void Initialize()
        {
        }

        [System.Obsolete("Main is deprecated, please use Draw instead.",false)]
        public virtual void Main(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

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
