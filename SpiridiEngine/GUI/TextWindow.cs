using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine.GUI
{
    public class TextWindow : Window
    {
        public enum Alignment { Left, Center, Right };

        private Alignment textAlign = Alignment.Left;
        private TextRenderer textRenderer = null;
        private string text = null;
        private List<string> lines = null;
        private bool wordWrap = false;
        private int margin = 0;
        private bool fitHeight = false;


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

        public Alignment TextAlign
        {
            get { return this.textAlign; }
            set { this.textAlign = value; }
        }

        public bool FitHeight
        {
            get { return this.fitHeight; }
            set { this.fitHeight = value; }
        }

        public string Text 
        {
            get { return this.text; }
            set
            {
                this.text = value;
                WrapText();
                AdjustSize();
            }
        }

        public bool WordWrap
        {
            get { return wordWrap; }
            set
            {
                wordWrap = value;
                WrapText();
                AdjustSize();
            }
        }

        public int Margin
        {
            get { return margin; }
            set
            {
                margin = value;
                WrapText();
                AdjustSize();
            }
        }

        private void WrapText()
        {
            if (this.wordWrap && this.textRenderer!= null)
            {
                this.lines = textRenderer.WrapString(this.text, this.Width - (this.margin + this.margin));
            }
        }

        private void AdjustSize()
        {
            if (this.fitHeight)
            {
                int numLines = 1;
                if (this.wordWrap && this.lines!= null)
                {
                    numLines = this.lines.Count;
                }

                int textHeight = 20;
                if (this.textRenderer != null)
                {
                    textHeight = textRenderer.LineHeight * numLines;
                }

                this.Height = (this.margin * 2) + textHeight;
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
                    int renderX = this.X;
                    switch (this.textAlign)
                    {
                        case(Alignment.Left):
                            renderX = this.X + margin;
                            break;
                        case(Alignment.Center):
                            int centerX = this.X + (this.Width / 2);
                            Vector2 centerExtents = textRenderer.MeasureText(line);
                            renderX = centerX - (int)(centerExtents.X / 2);
                            break;
                        case(Alignment.Right):
                            int rightX = this.X + this.Width;
                            Vector2 rightExtents = textRenderer.MeasureText(line);
                            renderX = rightX - (int)(rightExtents.X);
                            break;
                    }
                    

                    textRenderer.DrawText(spriteBatch, line, renderX, currentLineY);
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
