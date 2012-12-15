/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
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

        public bool KeyDown { get { return this.keysPressed.Count > 0; } }
        public bool KeyUp { get { return this.keysReleased.Count > 0; } }


        public bool KeyPressed(Keys key)
        {
            return keysPressed.Contains(key);
        }

        public bool KeyReleased(Keys key)
        {
            return keysReleased.Contains(key);
        }


        public void Update()
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
