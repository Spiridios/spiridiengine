/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Spiridios.SpiridiEngine
{
    public class SceneLayer : Drawable, Updatable
    {
        private List<Actor> actors = new List<Actor>();
        private IComparer<Actor> actorsComparer = new Actor.AscendingYComparitor();
        private string layerName = null;
        private Scene scene;
        private bool visible = true;

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
            if (this.visible)
            {
                SortActors();
                foreach (Actor actor in actors)
                {
                    actor.Draw(spriteBatch);
                }
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

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
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
