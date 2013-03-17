/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Spiridios.SpiridiEngine
{
    public class InputManager
    {
        public const string QUICK_EXIT_CONTROL = "QuickExit";

        private SpiridiGame game;
        private KeyboardEvent keyEvent;
        private Dictionary<string, Keys> continuousBindings = new Dictionary<string, Keys>();
        private Dictionary<string, Keys> momentaryBindings = new Dictionary<string, Keys>();
        
        public InputManager(SpiridiGame game)
        {
            this.game = game;
            keyEvent = new KeyboardEvent();

            this.RegisterMomentaryInput(QUICK_EXIT_CONTROL, Keys.Escape);
        }

        // TODO: this seems hacky.
        internal KeyboardEvent KeyboardEvent
        {
            get { return keyEvent; }
        }

        internal void Update(GameTime gameTime)
        {
            keyEvent.Update();

            // Send key events
            if (keyEvent.WereAnyKeysPressed)
            {
                this.game.CurrentState.KeyDown(keyEvent);
            }

            if (keyEvent.WereAnyKeysReleased)
            {
                this.game.CurrentState.KeyUp(keyEvent);
            }
        }

        public void RegisterMomentaryInput(string controlName, Keys keyBind)
        {
            momentaryBindings.Add(controlName, keyBind);
        }

        public bool IsTriggered(string controlName)
        {
            bool triggered = false;
            Keys binding;
            if (momentaryBindings.TryGetValue(controlName, out binding))
            {
                triggered = keyEvent.KeyPressed(binding);
            }
            return triggered;
        }

        public void RegisterContinuousInput(string controlName, Keys keyBind)
        {
            continuousBindings.Add(controlName, keyBind);
        }

        public bool IsActive(string controlName)
        {
            bool active = false;
            Keys binding;
            if (continuousBindings.TryGetValue(controlName, out binding))
            {
                active = keyEvent.IsKeyDown(binding);
            }
            return active;
        }

        public double GetCurrentRange(string controlName)
        {
            return 0.0;
        }
    }
}
