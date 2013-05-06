/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Spiridios.SpiridiEngine.Input;

namespace Spiridios.SpiridiEngine
{
    // TODO: combine SplashState with BootState since they both advance to a new state...
    public class SplashState : State
    {
        private State nextState;

        private string splashImageName;
        // TODO: This should not be sprite anymore
        private Sprite splashImage;

        private TimeSpan splashStart;
        private int updateCount = 0;
        private int splashDuration = 2000;

        public SplashState(SpiridiGame game, State nextState, string splashImage, int splashDurationSeconds)
            : base(game)
        {
            this.nextState = nextState;
            this.splashDuration = splashDurationSeconds;
            this.splashImageName = splashImage;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.splashImage = new Sprite(this.splashImageName);
            this.splashImage.Origin = new Vector2(0, 0);
            this.splashImage.Position = new Vector2((game.WindowWidth - this.splashImage.Width) / 2, (game.WindowHeight - this.splashImage.Height) / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (updateCount == 0)
            {
                splashStart = gameTime.TotalGameTime;
                updateCount++;
            }
            else if (updateCount == 1)
            {
                updateCount++;
                nextState.Initialize();
            }
            else if (gameTime.TotalGameTime.TotalMilliseconds - splashStart.TotalMilliseconds > splashDuration)
            {
                game.NextState = nextState;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.splashImage.Draw(game.SpriteBatch);
        }

        public override void KeyUp(KeyboardEvent keyState)
        {
            base.KeyUp(keyState);
            splashDuration = 0; //short-circuit the built-in delay
        }

    }
}
