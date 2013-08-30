using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine.GUI
{
    public class TextWindow : Window
    {
        private TextRenderer textRenderer = null;
        private string text = null;
        private List<string> lines = null;
        private bool wordWrap = false;
        private int margin = 0;

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
            this.textRenderer = textRenderer;
            this.text = text;
        }

        public TextRenderer TextRenderer
        {
            get { return this.textRenderer; }
            set { this.textRenderer = value; }
        }

        public string Text 
        {
            get { return this.text; }
            set
            {
                this.text = value;
                WrapText();
            }
        }

        public bool WordWrap
        {
            get { return wordWrap; }
            set
            {
                wordWrap = value;
                WrapText();
            }
        }

        public int Margin
        {
            get { return margin; }
            set
            {
                margin = value;
                WrapText();
            }
        }

        private void WrapText()
        {
            if (this.wordWrap && this.textRenderer!= null)
            {
                this.lines = textRenderer.WrapString(this.text, this.Width - (this.margin + this.margin));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.wordWrap)
            {
                int currentLineY = this.Y + margin;
                foreach (string line in lines)
                {
                    textRenderer.DrawText(spriteBatch, line, this.X + margin, currentLineY);
                    currentLineY += textRenderer.LineHeight;
                }
            }
            else
            {
                textRenderer.DrawText(spriteBatch, this.text, (int)this.Position.X, (int)this.Position.Y);
            }
        }
    }
}
