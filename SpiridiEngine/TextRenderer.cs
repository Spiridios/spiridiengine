using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

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
