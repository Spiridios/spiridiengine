/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Translates XNA's keyboard polling into generic Keyup/Keydown events
    /// like SDL uses.
    /// </summary>
    public class KeyboardEvent
    {
        private Keys[] lastKeys;
        private KeyboardState lastState;
        private KeyboardState currentState;
        private HashSet<Keys> keysReleased;
        private HashSet<Keys> keysPressed;

        public KeyboardEvent()
        {
            InitKeySets();
            lastState = Keyboard.GetState();
            lastKeys = GetKeyArray(lastState);
        }

        public KeyboardState KeyboardState { get { return currentState; } }

        /// <summary>
        /// Returns true if any keys were pressed since the last poll interval
        /// </summary>
        internal bool WereAnyKeysPressed { get { return this.keysPressed.Count > 0; } }
        /// <summary>
        /// Returns true if any keys were released since the last poll interval
        /// </summary>
        internal bool WereAnyKeysReleased { get { return this.keysReleased.Count > 0; } }

        /// <summary>
        /// Determines whether the given key was pressed since the last poll interval
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyPressed(Keys key)
        {
            return keysPressed.Contains(key);
        }

        /// <summary>
        /// Determines whether the given key was released since the last poll interval
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyReleased(Keys key)
        {
            return keysReleased.Contains(key);
        }

        /// <summary>
        /// Returns true if the specified key is currently pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if the specified key is currently released.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

        internal void Update()
        {
            this.lastState = this.currentState;

            this.currentState = Keyboard.GetState();
            
            Keys[] currentKeys = GetKeyArray(currentState);

            UpdateKeySets(currentKeys);

            this.lastKeys = currentKeys;
            
        }

        private static Keys[] GetKeyArray(KeyboardState keyState)
        {
            Keys[] keys = keyState.GetPressedKeys();
            Array.Sort(keys);
            return keys;
        }

        private void InitKeySets()
        {
            keysReleased = new HashSet<Keys>();
            keysPressed = new HashSet<Keys>();
        }

        /// <summary>
        /// Determine which keys were pressed or released during the last update
        /// </summary>
        /// <param name="currentKeys"></param>
        private void UpdateKeySets(Keys[] currentKeys)
        {
            InitKeySets();
            int lastIndex = 0;
            int currentIndex = 0;
            while ((lastIndex < lastKeys.Length || currentIndex < currentKeys.Length))
            {
                if (lastIndex >= lastKeys.Length)
                {
                    this.keysPressed.Add(currentKeys[currentIndex]);
                    currentIndex++;
                }
                else if (currentIndex >= currentKeys.Length)
                {
                    this.keysReleased.Add(lastKeys[lastIndex]);
                    lastIndex++;
                }
                else
                {
                    if (lastKeys[lastIndex] < currentKeys[currentIndex])
                    {
                        this.keysReleased.Add(lastKeys[lastIndex]);
                        lastIndex++;
                    }
                    else if (lastKeys[lastIndex] > currentKeys[currentIndex])
                    {
                        this.keysPressed.Add(currentKeys[currentIndex]);
                        currentIndex++;
                    }
                    else // (lastKeys[lastIndex] == currentKeys[currentIndex])
                    {
                        //no change
                        lastIndex++;
                        currentIndex++;
                    }
                }
            }
        }
    }
}
