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

        public TextRenderer(SpiridiGame game, String fontName, Color fontColor)
        {
            this.game = game;
            this.font = game.Content.Load<SpriteFont>(fontName);
            this.color = fontColor;
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

            spriteBatch.DrawString(this.font, text, position, this.color);
        }
    }
}
