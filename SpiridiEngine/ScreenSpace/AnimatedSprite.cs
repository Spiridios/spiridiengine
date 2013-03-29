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
    public class AnimatedSprite : Sprite
    {
        private AnimatedImage image = null;

        public AnimatedSprite(string imageName)
            : base()
        {
            image = new AnimatedImage(imageName);
            image.Origin = new Vector2(image.Width / 2.0f, image.Height / 2.0f);
        }

        public AnimatedSprite(AnimatedImage image)
        {
            this.image = image;
            this.image.Origin = new Vector2(image.Width / 2.0f, image.Height / 2.0f);
        }

        public int CurrentFrame
        {
            get { return image.CurrentFrame; }
            set { image.CurrentFrame = value; }
        }

        public string CurrentAnimation
        {
            get { return image.CurrentAnimation; }
            set { image.CurrentAnimation = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch, this.Position, this.TintColor, this.Rotation, this.Layer);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            image.Update(elapsedTime);
        }

        public override Vector2 CenterOffset
        {
            get { return this.image.Origin; }
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Height
        {
            get { return image.Height; }
        }
    }
}
