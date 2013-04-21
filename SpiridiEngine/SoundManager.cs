/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;


namespace Spiridios.SpiridiEngine
{

    public class SoundManager
    {
        private static SoundManager instance;

        public static SoundManager Instance
        {
            get { return instance; }
        }

        internal static void SetInstance(SoundManager instance)
        {
            SoundManager.instance = instance;
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
            string correctedName = SpiridiGame.NormalizeFilenameXNA(soundFile);
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
