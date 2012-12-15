/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class Actor : Object2d
    {
        public enum LifeStage
        {
            ALIVE, DYING, DEAD
        };

        private Sprite sprite = null;

        public bool Collidable { get; set; }
        public LifeStage lifeStage { get; set; }
        protected float BoundingRadius { get; set; }
        private Vector2 centerOffset;

        private Dictionary<LifeStage, Behavior> stageBehaviors = new Dictionary<LifeStage, Behavior>();

        public Actor(string imageName)
            : this(new StaticSprite(imageName))
        {
        }

        public Actor(Sprite sprite)
        {
            this.sprite = sprite;
            centerOffset = new Vector2(this.Width / 2.0f, this.Height / 2.0f);

            Position = new Vector2(0, 0);
            Rotation = 0.0f;
            Collidable = true;
            lifeStage = LifeStage.ALIVE;
            this.BoundingRadius = this.Width > this.Height ? this.Width / 2.0f : this.Height / 2.0f;
            this.TintColor = Color.White;
            this.Layer = 0.0f;
        }

        public Color TintColor
        {
            get { return sprite.TintColor;  }
            set { sprite.TintColor = value; }
        }

        public float Rotation
        {
            get { return sprite.Rotation; }
            set { sprite.Rotation = value; }
        }

        public float Layer
        {
            get { return sprite.Layer; }
            set { sprite.Layer = value; }
        }

        public void SetBehavior(LifeStage stage, Behavior behavior)
        {
            this.stageBehaviors[stage] = behavior;
        }

        public bool IsALive
        {
            get { return this.lifeStage == LifeStage.ALIVE; }
        }

        public bool IsDead
        {
            get { return this.lifeStage == LifeStage.DEAD; }
        }

        public int Width
        {
            get { return this.sprite.Width; }
        }

        public int Height
        {
            get { return this.sprite.Height; }
        }

        public Vector2 GetCenter()
        {
            return Vector2.Add(this.Position, this.GetCenterOffset());
        }

        // TODO: should this BE public?
        public Vector2 GetCenterOffset()
        {
            return this.centerOffset;
        }

        //TODO: refactor into collidable or base object or something.
        public bool CollidesWith(Actor that)
        {
            if (this.Collidable)
            {
                Vector2 thisCenter = this.GetCenter();
                Vector2 thatCenter = that.GetCenter();

                Vector2 direction = thisCenter - thatCenter;
                float length = direction.Length();
                float thisRadius = this.BoundingRadius;
                float thatRadius = that.BoundingRadius;
                return !(length > (thisRadius + thatRadius));
            }
            else
            {
                return false;
            }
        }

        // TODO: Refactor this into a base default behavior.
        public override void Update(TimeSpan elapsedTime)
        {
            if (stageBehaviors.ContainsKey(this.lifeStage))
            {
                Behavior b = stageBehaviors[this.lifeStage];
                b.Update(elapsedTime);
            }
            else
            {
                this.sprite.Update(elapsedTime);
            }

        }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            this.DrawSprite(spriteBatch, this.Position);
        }

        public void DrawSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            sprite.Draw(spriteBatch, position, centerOffset);
            //spriteBatch.Draw(this.image, position + this.centerOffset, null, this.TintColor, this.Rotation, this.centerOffset, 1.0f, SpriteEffects.None, this.Layer);
        }

        // TODO: move into default behavior.
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (stageBehaviors.ContainsKey(this.lifeStage))
            {
                Behavior b = stageBehaviors[this.lifeStage];
                b.Draw(spriteBatch);
            }
            else if(!this.IsDead)
            {
                this.DrawSprite(spriteBatch);
            }
        }

        public static void UpdateList(List<Actor> sprites, TimeSpan elapsedTime)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                Actor s = sprites[i];
                s.Update(elapsedTime);
                if (s.IsDead)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }

        }

        public static void DrawList(List<Actor> drawables, SpriteBatch spriteBatch)
        {
            foreach (Drawable d in drawables)
            {
                d.Draw(spriteBatch);
            }
        }

    }
}
