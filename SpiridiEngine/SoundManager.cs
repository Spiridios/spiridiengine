using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Spiridios.SpiridiEngine
{

    public class SoundManager
    {
        private static SoundManager instance;

        public static SoundManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        ContentManager contentManager;

        internal SoundManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        /// <summary>
        /// Adds a sound to the sound manager
        /// </summary>
        /// <param name="soundName">The internal name for the sound</param>
        /// <param name="soundFile">The actual sound name</param>
        public void AddSound(string soundName, string soundFile)
        {
            string correctedName = SpiridiGame.NormalizeFilename(soundFile);
            try
            {
                SoundEffect snd = this.contentManager.Load<SoundEffect>(correctedName);
                this.soundEffects[soundName] = snd;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Could not load sound '{0}' - {1}", soundFile, e.Message));
            }

        }

        public SoundEffect GetSound(string soundName)
        {
            return this.soundEffects[soundName];
        }

        public void PlaySound(string soundName)
        {
            GetSound(soundName).Play();
        }

        internal void Clear()
        {
            this.soundEffects.Clear();
        }
    }
}
