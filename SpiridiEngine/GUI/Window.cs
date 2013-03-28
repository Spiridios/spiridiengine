﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spiridios.SpiridiEngine.GUI
{
    public abstract class Window : ScreenObject
    {

        public override void Update(TimeSpan elapsedTime) {}
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {}
    }
}
