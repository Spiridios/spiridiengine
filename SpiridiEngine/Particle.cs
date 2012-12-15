/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class Particle : Actor
    {
        private Vector2 velocity;
        private double age = 0;
        private double lifeSpan;
        private Color startColor;
        private Color endColor;

        public static Particle CreateExplosionParticle(string imageName, Vector2 position)
        {
            Builder b = new Builder(imageName);
            b.Position = position;
            b.LifeSpan = 1;
            b.LifeSpanVariance = 1;
            b.Speed = 100;
            b.SpeedVariance = 100;
            b.StartColor = Color.Red;
            Color e = Color.Red;
            e.A = 0x0;
            b.EndColor = e;

            return b.Build();
        }

        private Particle(string imageName) : base(imageName)
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            age += elapsedTime.TotalSeconds;
            if (age > lifeSpan)
            {
                this.TintColor = this.endColor;
                this.lifeStage = LifeStage.DEAD;
            }
            else
            {
                this.TintColor = Color.Lerp(startColor, endColor, (float)(age/lifeSpan));
            }

            this.Position += (this.velocity * (float)elapsedTime.TotalSeconds);
        }

        public class Builder
        {
            // Options:
            //  lifeSpan
            //  lifeSpan perturbation
            //  Speed
            //  spawn point
            //  spawn radius (can spawn anywhere in this radius)
            //  cloud velocity (an overall velocity of the particles)
            //  emission velocity (average velocity of particles from spawn point)
            //  emission perturbation (difference in emission speed)

            public string ImageName { get; set; }
            public Vector2 Position { get; set; }
            // Seconds
            public double LifeSpan { get; set; }
            public double LifeSpanVariance { get; set; }
            // Pixels/second
            public double Speed { get; set; }
            public double SpeedVariance { get; set; }

            public Color StartColor { get; set; }
            public Color EndColor { get; set; }

            public Builder(string imageName)
            {
                this.ImageName = imageName;
                this.LifeSpan = 0;
                this.LifeSpanVariance = 0;
                this.Speed = 0;
                this.SpeedVariance = 0;
                this.StartColor = Color.White;
                this.EndColor = Color.White;
            }

            public Particle Build()
            {
                Vector2 velocity = new Vector2(0, 1) * (float)(Speed + (SpiridiGame.Random.NextDouble() * 2 * SpeedVariance) - SpeedVariance);
                float theta = (float)(SpiridiGame.Random.NextDouble() * (2 * Math.PI));
                velocity = Vector2Ext.Rotate(velocity, theta);

                double actualLifeSpan = LifeSpan + (SpiridiGame.Random.NextDouble() * 2 * LifeSpanVariance) - LifeSpanVariance; 
                return new Particle(ImageName)
                {
                    Position = Position,
                    lifeSpan = actualLifeSpan,
                    velocity = velocity,
                    startColor = StartColor,
                    endColor = EndColor
                };
            }

        }

    }
}
