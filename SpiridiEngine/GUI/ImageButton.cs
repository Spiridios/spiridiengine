using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spiridios.SpiridiEngine.GUI
{
    public class ImageButton : Window
    {
        private AnimatedSprite buttonSprite;

        public ImageButton(AnimatedSprite buttonSprite, int upFrame, int downFrame)
        {
            this.buttonSprite = buttonSprite;
            buttonSprite.CurrentFrame = upFrame;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            // swallow it for now.
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            buttonSprite.Draw(spriteBatch, this.Position);
        }

    }
}
