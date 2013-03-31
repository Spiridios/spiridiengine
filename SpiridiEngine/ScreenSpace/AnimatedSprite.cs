/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    [System.Obsolete("AnimatedSprite is deprecated. Use Sprite with AnimatedImage instead.", true)]
    public class AnimatedSprite : Sprite
    {
        private AnimatedImage animatedImage = null;

        public AnimatedSprite(string imageName)
            : this(new AnimatedImage(imageName))
        {
        }

        public AnimatedSprite(AnimatedImage image)
            : base(image)
        {
            this.animatedImage = image;
        }

        public int CurrentFrame
        {
            get { return animatedImage.CurrentFrame; }
            set { animatedImage.CurrentFrame = value; }
        }

        public string CurrentAnimation
        {
            get { return animatedImage.CurrentAnimation; }
            set { animatedImage.CurrentAnimation = value; }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            animatedImage.Update(elapsedTime);
        }

    }
}
