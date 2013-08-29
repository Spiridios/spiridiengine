using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine.GUI
{
    public class TextWindow : Window
    {
        private TextRenderer renderer;
        private string text;

        public TextWindow()
            : this(SpiridiGame.Instance.DefaultTextRenderer, null) 
        {
        }

        public TextWindow(string text)
            : this(SpiridiGame.Instance.DefaultTextRenderer, text) 
        {
        }

        public TextWindow(TextRenderer textRenderer)
            : this(textRenderer, null)
        {
        }

        public TextWindow(TextRenderer textRenderer, string text)
        {
            this.renderer = textRenderer;
            this.text = text;
        }

        public TextRenderer TextRenderer
        {
            get { return this.renderer; }
            set { this.renderer = value; }
        }

        public string Text 
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            renderer.DrawText(spriteBatch, this.text, (int)this.Position.X, (int)this.Position.Y);
        }
    }
}
