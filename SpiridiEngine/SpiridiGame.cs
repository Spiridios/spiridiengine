/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Spiridios.SpiridiEngine
{
    public abstract class SpiridiGame : Microsoft.Xna.Framework.Game
    {
        private static Random random = new Random();
        private static ImageManager imageManager = null;

        public static readonly Color DefaultClearColor = new Color(0xff, 0x80, 0xc0);
        private Color clearColor = DefaultClearColor;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Song backgroundMusic;
        private State currentState;
        public TextRenderer DefaultTextRenderer { get; set; }
        private KeyboardEvent keyEvent;
        private SpriteSortMode spriteSortMode { get; set; }
        private int windowWidth;
        private int windowHeight;

        public bool ShowFPS { get; set; }
        private TimeSpan fpsTimer;
        private int frameCount = -1;
        private float fps = 0.0f;

        public SpiridiGame()
        {
            this.spriteSortMode = SpriteSortMode.Deferred;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SetWindowSize(640, 480);
            if (imageManager == null)
            {
                imageManager = new ImageManager(Content);
            }

            SoundManager.Instance = new SoundManager(Content);
            this.currentState = new State(this);
        }

        /// <summary>
        /// Gets the game's single random number generator.
        /// Useful if a predetermined key has been fed to the generator.
        /// </summary>
        public static Random Random
        {
            get { return random; }
        }

        public ImageManager ImageManager
        {
            get { return SpiridiGame.imageManager; }
        }

        public static ImageManager ImageManagerInstance
        {
            get { return SpiridiGame.imageManager; }
        }

        public static String NormalizeFilename(String fileName)
        {
            return System.IO.Path.GetFileNameWithoutExtension(fileName);
        }

        public Color ClearColor
        {
            get { return this.clearColor; }
            set { this.clearColor = value; }
        }

        public float FPS
        {
            get { return this.fps; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public State NextState 
        {
            get { return currentState; }
            set { currentState = value; }
        }

        protected void SetWindowSize(int width, int height)
        {
            windowWidth = graphics.PreferredBackBufferWidth = width;
            windowHeight = graphics.PreferredBackBufferHeight = height;
        }

        public int WindowWidth
        {
            get { return windowWidth; }
        }

        public int WindowHeight
        {
            get { return windowHeight; }
        }

        protected SoundEffect LoadSound(String soundFile)
        {
            return Content.Load<SoundEffect>(NormalizeFilename(soundFile));
        }

        protected void PlayBackgroundMusic(String musicFile)
        {
            this.backgroundMusic = Content.Load<Song>(NormalizeFilename(musicFile));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(this.backgroundMusic);
        }

        public abstract void InitObjects();

        protected override void LoadContent()
        {
            base.LoadContent();
            imageManager.Clear();
        }

        protected override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            keyEvent = new KeyboardEvent();
        }


        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Get key/mouse state?
            keyEvent.Update();
            if (keyEvent.KeyDown)
            {
                this.currentState.KeyDown(keyEvent);
            }

            if (keyEvent.KeyUp)
            {
                this.currentState.KeyUp(keyEvent);
            }

            this.currentState.Update(gameTime);
        }

        public void DrawText(String str, int x, int y)
        {
            DefaultTextRenderer.DrawText(spriteBatch, str, x, y);
        }

        public void DrawFPS()
        {
            if (this.ShowFPS)
            {
                this.DrawText(String.Format("{0:0.0} FPS", this.fps),2,2);
            }
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.clearColor);

            if (frameCount < 0)
            {
                this.fpsTimer = gameTime.TotalGameTime;
                this.frameCount = 0;
            }

            double fpsSpan = gameTime.TotalGameTime.TotalSeconds - this.fpsTimer.TotalSeconds;
            if (fpsSpan > 1)
            {
                this.fps = (float)(this.frameCount / fpsSpan);
                this.fpsTimer = gameTime.TotalGameTime;
                this.frameCount = 0;
            }

            this.frameCount++;

#if(!SILVERLIGHT)
            spriteBatch.Begin(this.spriteSortMode, BlendState.NonPremultiplied);
#else
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, this.spriteSortMode, SaveStateMode.None);
#endif

            this.currentState.Main(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
