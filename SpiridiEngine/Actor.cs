﻿using System;
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

        internal static ImageManager imageManager = null;

        private Texture2D image = null;

        public float Rotation { get; set; }
        public bool Collidable { get; set; }
        public LifeStage lifeStage { get; set; }
        protected float BoundingRadius { get; set; }
        private Vector2 centerOffset;
        public Color TintColor { get; set; }
        public float Layer { get; set; }

        private Dictionary<LifeStage, Behavior> stageBehaviors = new Dictionary<LifeStage, Behavior>();


        public Actor(string imageName)
        {
            image = imageManager.GetImage(imageName);
            centerOffset = new Vector2(this.Width / 2.0f, this.Height / 2.0f);

            Position = new Vector2(0, 0);
            Rotation = 0.0f;
            Collidable = true;
            lifeStage = LifeStage.ALIVE;
            this.BoundingRadius = this.Width > this.Height ? this.Width/2.0f : this.Height/2.0f;
            this.TintColor = Color.White;
            this.Layer = 0.0f;
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
            get { return this.image.Width; }
        }

        public int Height
        {
            get { return this.image.Height; }
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

        public override void Update(TimeSpan elapsedTime)
        {
            if (stageBehaviors.ContainsKey(this.lifeStage))
            {
                Behavior b = stageBehaviors[this.lifeStage];
                b.Update(elapsedTime);
            }

        }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            this.DrawSprite(spriteBatch, this.Position);
        }

        public void DrawSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
            spriteBatch.Draw(this.image, this.Position + this.centerOffset, null, this.TintColor, this.Rotation, this.centerOffset, 1.0f, SpriteEffects.None, this.Layer);
        }

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
