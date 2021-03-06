﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spiridios.SpiridiEngine.Audio
{
    public class PositionedSound : WorldObject
    {
        private Image image = null;
        private SoundEffectInstance soundEffectInstance;
        private Actor listener = null;
        private float attenuationFactor = 1.0f;
        private static readonly Vector2 FORWARD = new Vector2(0, 1);
        private float maxVolume = 1.0f;

        private const float MAX_DISTANCE = 50.0f;


        public PositionedSound(SoundEffect soundEffect)
            : this(soundEffect.CreateInstance())
        {
        }

        public PositionedSound(SoundEffectInstance soundEffectInstance)
        {
            this.soundEffectInstance = soundEffectInstance;
        }

        public float AttenuationFactor
        {
            get { return this.attenuationFactor; }
            set { this.attenuationFactor = value; }
        }

        public float Volume
        {
            get { return this.maxVolume; }
            set { this.maxVolume = Math.Min(value, 1.0f); }
        }

        public Actor Listener
        {
            set
            {
                this.listener = value;
                this.UpdateListener();
            }
        }

        public Image DebugImage
        {
            set { this.image = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                image.Draw(spriteBatch, this.ScreenPosition);
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (listener != null)
            {
                UpdateListener();
            }
        }

        public void Play()
        {
            soundEffectInstance.IsLooped = false;
            soundEffectInstance.Play();
        }

        public void PlayLooped()
        {
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();
        }

        public void Stop()
        {
            soundEffectInstance.Stop();
        }

        public bool IsPlaying
        {
            get { return soundEffectInstance.State == SoundState.Playing; }
        }

        private void UpdateListener()
        {
            UpdateListener(listener.Position, Vector2Ext.Rotate(FORWARD, listener.Rotation));
        }
        private void UpdateListener(Vector2 position, Vector2 orientation)
        {
            Vector2 vectorToSound = position - this.Position;
            float distance = vectorToSound.Length();
            // Linear
            //float volume = Math.Max(1 - (distance / MAX_DISTANCE), 0.0f)
            
            // Exponential
            float volume = Math.Min(1 / (distance * attenuationFactor), this.maxVolume);

            // Exponetial square
            //float volume = Math.Min(1 / (distance * distance * attenuationFactor), this.maxVolume);

            float pan = 0.0f;

            float angle = (float)(Vector2Ext.AngleOf(orientation, vectorToSound));
            if (angle > MathHelper.Pi)
            {
                angle -= MathHelper.TwoPi;
            }
            else if (angle < -MathHelper.Pi)
            {
                angle += MathHelper.TwoPi;
            }

            pan = (float)Math.Sin(angle);
            float sign = Math.Sign(pan);
            pan = (float)Math.Sqrt(Math.Abs(pan));

            pan = pan * sign;

            if (Math.Abs(angle) > MathHelper.PiOver2)
            {
                volume = volume * .5f;
            }

            soundEffectInstance.Volume = volume;
            soundEffectInstance.Pan = pan;
        }

    }
}
