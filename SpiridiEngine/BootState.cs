using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class BootState : State
    {
        private State nextState;

        public BootState(SpiridiGame game, State nextState)
            : base(game)
        {
            this.nextState = nextState;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            nextState.Initialize();
            game.NextState = nextState;
        }
    }
}
