using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Spiridios.SpiridiEngine
{
    public class SceneLayer : Drawable, Updatable
    {
        // TODO: This shouldn't be protected
        protected List<Actor> actors = new List<Actor>();
        protected IComparer<Actor> actorsComparer = new Actor.AscendingYComparitor();
        private string layerName;

        public void AddActor(Actor actor)
        {
            this.actors.Add(actor);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            actors.Sort(actorsComparer);
            foreach (Actor actor in actors)
            {
                actor.Draw(spriteBatch);
            }
        }

        public virtual void Update(System.TimeSpan elapsedTime)
        {
            foreach (Actor actor in actors)
            {
                actor.Update(elapsedTime);
            }
        }

        public string Name
        {
            get { return this.layerName; }
            set { this.layerName = value; }
        }
        
    }
}
