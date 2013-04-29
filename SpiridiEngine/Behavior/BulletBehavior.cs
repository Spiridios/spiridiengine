/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Spiridios.SpiridiEngine.Physics;
using Spiridios.SpiridiEngine.Scene;

namespace Spiridios.SpiridiEngine
{
    public class BulletBehavior : Behavior, CollisionListener
    {
        private Vector2 velocity;
        private SpiridiGame game;
        public const string COLLIDABLE_TAG = "BulletActor";

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

            bullet.Position = startPosition - bullet.Origin;
            BulletBehavior b = new BulletBehavior(bullet, velocity, game);
            bullet.SetBehavior(Actor.LifeStage.ALIVE, b);

            bullet.Collidable.Tag = COLLIDABLE_TAG;
            bullet.Collidable.AddCollisionListener(b);

            SoundManager.Instance.PlaySound(laserSound);

            return bullet;
        }

        void CollisionListener.OnCollided(System.Collections.Generic.List<Collidable> activeCollidables)
        {
            foreach (Collidable collidable in activeCollidables)
            {
                if (collidable.Tag == TileMapLayer.COLLIDABLE_TAG)
                {
                    this.Actor.lifeStage = Actor.LifeStage.DEAD;
                }
            }
        }
    }
}
