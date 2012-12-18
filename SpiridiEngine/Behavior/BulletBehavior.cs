using System;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class BulletBehavior : Behavior
    {
        private Vector2 velocity;
        private SpiridiGame game;

        public BulletBehavior(Actor actor, Vector2 velocity, SpiridiGame game)
            : base(actor)
        {
            this.velocity = velocity;
            this.game = game;
            this.Actor.Rotation = (float)System.Math.Atan2(this.velocity.X, -this.velocity.Y);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if (this.Actor.IsALive)
            {
                this.Actor.Position = this.Actor.Position + (this.velocity * (float)elapsedTime.TotalSeconds);
                if (
                    this.Actor.Position.X < -this.Actor.Width ||
                    this.Actor.Position.Y < -this.Actor.Height ||
                    this.Actor.Position.X > this.game.WindowWidth ||
                    this.Actor.Position.Y > this.game.WindowHeight
                   )
                {
                    this.Actor.lifeStage = Actor.LifeStage.DEAD;
                }
            }
        }

        public static Actor CreateBullet(string imageName, Vector2 startPosition, Vector2 velocity, string laserSound, SpiridiGame game)
        {
            Actor bullet = new Actor(imageName);
            bullet.Position = startPosition - bullet.GetCenterOffset();
            bullet.SetBehavior(Actor.LifeStage.ALIVE, new BulletBehavior(bullet, velocity, game));
            SoundManager.Instance.PlaySound(laserSound);
            return bullet;
        }
    }
}
