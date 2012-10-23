using System;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class Behavior : Updatable, Drawable
    {
        private Actor actor;
        protected Actor Actor
        {
            get { return actor; }
        }

        public Behavior(Actor actor)
        {
            this.actor = actor;
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            actor.DrawSprite(spriteBatch);
        }
    }
}
