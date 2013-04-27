using System;
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

        public PositionedSound(SoundEffect soundEffect)
            : this(soundEffect.CreateInstance())
        {
        }

        public PositionedSound(SoundEffectInstance soundEffectInstance)
        {
            this.soundEffectInstance = soundEffectInstance;
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
            // Nothing yet
        }

        public void Play(Vector2 position, Vector2 orientation)
        {
            soundEffectInstance.Volume = 1.0f;
            soundEffectInstance.Pan = 0.0f;
            soundEffectInstance.Play();
        }
    }
}
