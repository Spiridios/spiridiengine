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

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            Actor.Position = Actor.Position + (this.velocity * (float)elapsedTime.TotalSeconds);
        }
    }
}
