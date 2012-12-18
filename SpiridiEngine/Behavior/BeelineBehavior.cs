/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class BeelineBehavior : Behavior
    {
        private Vector2 velocity;

        public BeelineBehavior(Actor actor, Vector2 target, float speed)
            : base(actor)
        {
            this.velocity = target - actor.Position;
            velocity.Normalize();
            velocity = velocity * speed;
        }

        public BeelineBehavior(Actor actor, Vector2 velocity)
            : base(actor)
        {
            this.velocity = velocity;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            Actor.Position = Actor.Position + (this.velocity * (float)elapsedTime.TotalSeconds);
        }
    }
}
