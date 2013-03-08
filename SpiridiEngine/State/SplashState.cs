using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    // TODO: combine SplashState with BootState since the both advance to a new state...
    public class SplashState : State
    {
        private State nextState;

        private string splashImageName;
        private Image splashImage;

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
            this.splashImage = new Image(this.splashImageName);
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
