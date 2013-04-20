/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spiridios.SpiridiEngine.Physics;

namespace Spiridios.SpiridiEngine
{
    public class Actor : WorldObject
    {
        public class AscendingYComparitor : IComparer<Actor>
        {
            public int Compare(Actor a, Actor b)
            {
                if (a.Position.Y < b.Position.Y)
                {
                    return -1;
                }
                else if (a.Position.Y > b.Position.Y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public enum LifeStage
        {
            ALIVE, DYING, DEAD
        };

        private Sprite sprite = null;
        private Collidable collidable = null;

        public LifeStage lifeStage { get; set; }
        protected float BoundingRadius { get; set; }

        private Dictionary<LifeStage, Behavior> stageBehaviors = new Dictionary<LifeStage, Behavior>();

        public Actor(string imageName)
            : this(new Sprite(imageName))
        {
        }

        public Actor(Image image)
            : this(new Sprite(image))
        {
        }

        public Actor(Sprite sprite)
        {
            this.sprite = sprite;
            AddScreenObject(sprite);

            Position = new Vector2(0, 0);

            Rotation = 0.0f;
            lifeStage = LifeStage.ALIVE;

            this.collidable = new Collidable();
            this.collidable.RadiusCollidableShape = new RadiusCollidableShape(Position, this.Width > this.Height ? this.Width / 2.0f : this.Height / 2.0f);
            //this.collidable.BoxCollidableShape = new BoxCollidableShape((int)Position.X, (int)Position.Y, this.Width, this.Height);

            this.TintColor = Color.White;
            this.Layer = 0.0f;
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, this.Width, this.Height); }
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

        protected Sprite Sprite
        {
            get { return this.sprite; }
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
            return Vector2.Add(this.PositionHandler.ScreenPosition, this.Origin);
        }

        public Vector2 Origin
        {
            get { return this.sprite.Origin; }
        }

        public Collidable Collidable
        {
            get { return this.collidable; }
            set { this.collidable = value; }
        }

        //TODO: CollidesWith uses ScreenSpace coordinates
        public bool CollidesWith(Actor that)
        {
            if (this.collidable != null)
            {
                return this.collidable.CollidesWith(that.collidable);
            }
            else
            {
                return false;
            }
        }

        // TODO: Refactor this into a base default behavior.
        public override void Update(TimeSpan elapsedTime)
        {
            // TODO: this is bad - need better seperation between actor and behavior.
            this.sprite.Update(elapsedTime);

            if (stageBehaviors.ContainsKey(this.lifeStage))
            {
                Behavior b = stageBehaviors[this.lifeStage];
                b.Update(elapsedTime);
            }

            if (this.collidable != null)
            {
                if (this.collidable.RadiusCollidableShape != null)
                {
                    this.collidable.RadiusCollidableShape.Position = this.GetCenter();
                }
                if (this.collidable.BoxCollidableShape != null)
                {
                    Rectangle r = this.collidable.BoxCollidableShape.Rectangle;
                    r.X = (int)this.Position.X;
                    r.Y = (int)this.Position.Y;
                    this.collidable.BoxCollidableShape.Rectangle = r;
                }
            }
        }

        // TODO: internal?! Behavior calls it, but still....
        internal void DrawSprite(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
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
