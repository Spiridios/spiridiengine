using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Spiridios.SpiridiEngine
{
    public class SceneLayer : Drawable, Updatable
    {
        // TODO: This shouldn't be protected
        private List<Actor> actors = new List<Actor>();
        private IComparer<Actor> actorsComparer = new Actor.AscendingYComparitor();
        private string layerName;
        private Scene scene;

        public SceneLayer(Scene scene)
        {
            this.scene = scene;
        }

        public void AddActor(Actor actor)
        {
            this.actors.Add(actor);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            SortActors();
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

        public List<Actor> Actors
        {
            get { return actors; }
        }

        public Scene Scene
        {
            get { return scene; }
        }

        public void SortActors()
        {
            actors.Sort(actorsComparer);
        }

        public string Name
        {
            get { return this.layerName; }
            set { this.layerName = value; }
        }

        public virtual void ProcessCollisions()
        {

        }
    }
}
