using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class Explosion : Behavior
    {
        private List<Actor> particles = new List<Actor>();
        private int numParticles;
        private bool exploding = false;

        public Explosion(Actor actor, int numParticles)
            : base(actor)
        {
            this.numParticles = numParticles;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            if (!this.exploding)
            {
                Explode();
            }

            Actor.UpdateList(this.particles, elapsedTime);
            if (this.particles.Count == 0)
            {
                Actor.lifeStage = Actor.LifeStage.DEAD;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Actor.DrawList(particles, spriteBatch);
        }

        private void Explode()
        {
            Vector2 position = this.Actor.GetCenter();
            this.exploding = true;
            for (int i = 0; i < this.numParticles; i++)
            {
                Actor spr = Particle.CreateExplosionParticle("particle", position);
                this.particles.Add(spr);
            }
            SoundManager.Instance.PlaySound("explosion");
        }
    }
}
