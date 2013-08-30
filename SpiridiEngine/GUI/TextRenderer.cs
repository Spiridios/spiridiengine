/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;

namespace Spiridios.SpiridiEngine
{
    public class TextRenderer
    {
        public const int CENTERED = -1;
        public const int LEFT = -2;
        public const int RIGHT = -3;

        private SpriteFont font;
        private Color color;
        private SpiridiGame game;
        public bool DropShadow { get; set; }
        private Color dropShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        private Vector2 dropShadowOffset = new Vector2(1, 1);

        public TextRenderer(SpiridiGame game, String fontName, Color fontColor)
        {
            this.DropShadow = false;
            this.game = game;
            this.font = game.Content.Load<SpriteFont>(fontName);
            this.color = fontColor;
        }

        public Color TextColor
        {
            get { return color;}
            set { color = value; }
        }

        public Color DropShadowColor
        {
            get { return dropShadowColor; }
            set { dropShadowColor = value; }
        }

        public Vector2 DropShadowOffset
        {
            get { return dropShadowOffset; }
            set { dropShadowOffset = value; }
        }

        public void DrawText(SpriteBatch spriteBatch, String text, int x, int y)
        {
            Vector2 extents = this.font.MeasureString(text);
            Vector2 position = new Vector2(x, y);
            if (x < 0)
            {
                switch (x)
                {
                    case LEFT:
                        position.X = 0;
                        break;
                    case RIGHT:
                        position.X = game.WindowWidth - extents.X;
                        break;
                    case CENTERED:
                        position.X = (game.WindowWidth - extents.X)/2;
                        break;
                }
            }

            if (this.DropShadow)
            {
                spriteBatch.DrawString(this.font, text, position + dropShadowOffset, this.dropShadowColor);
            }
            spriteBatch.DrawString(this.font, text, position, this.color);
        }

        public int LineHeight
        {
            get { return this.font.LineSpacing; }
        }

        public List<string> WrapString(string text, int width)
        {
            List<string> wrappedLines = new List<string>();

            string[] hardLines = text.Split('\n');
            if (!String.IsNullOrEmpty(text))
            {
                foreach (string hardLine in hardLines)
                {
                    String[] words = hardLine.Split(' ');
                    StringBuilder wrappedLine = new StringBuilder();
                    int lineWordCount = 0;

                    foreach (string word in words)
                    {
                        if (lineWordCount > 0)
                        {
                            wrappedLine.Append(' ');
                        }
                        wrappedLine.Append(word);
                        lineWordCount++;

                        Vector2 extents = this.font.MeasureString(wrappedLine);
                        if (extents.X > width)
                        {
                            if (lineWordCount > 1)
                            {
                                // +/-1 for space that's added.
                                wrappedLine.Remove(wrappedLine.Length - word.Length - 1, word.Length + 1);
                                wrappedLines.Add(wrappedLine.ToString());
                                wrappedLine.Clear();
                                wrappedLine.Append(word);
                                lineWordCount = 1;
                            }
                            else
                            {
                                wrappedLines.Add(wrappedLine.ToString());
                                wrappedLine.Clear();
                                lineWordCount = 0;
                            }
                        }
                    }
                    wrappedLines.Add(wrappedLine.ToString());
                }
            }
            return wrappedLines;
        }
    }
}
