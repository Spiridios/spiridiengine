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
using Microsoft.Xna.Framework.Input;

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
        private SpriteSortMode SpriteSortMode { get; set; }
        private int windowWidth;
        private int windowHeight;

        public bool IsQuickExit { get; set; }

        public bool ShowFPS { get; set; }
        private TimeSpan fpsTimer;
        private int frameCount = -1;
        private float fps = 0.0f;
        public Vector2 FPSPosition { get; set; }

        public SpiridiGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            SoundManager.SetInstance(new SoundManager(Content));

            if (imageManager == null)
            {
                imageManager = new ImageManager(Content);
            }

            SetWindowSize(640, 480);

            this.currentState = new State(this);
            this.SpriteSortMode = SpriteSortMode.Deferred;
            this.FPSPosition = new Vector2(2, 2);
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
            // NOTE: See http://stackoverflow.com/questions/720429/how-do-i-set-the-window-screen-size-in-xna
            // graphics.ApplyChanges() needs to be called if this is changed outside the constructor.
            // Also see :http://xboxforums.create.msdn.com/forums/p/1031/107718.aspx
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

        public void PlayBackgroundMusic(String musicFile)
        {
            this.backgroundMusic = Content.Load<Song>(NormalizeFilename(musicFile));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(this.backgroundMusic);
        }

        public void StopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }

        // TODO: I don't think this is needed anymore.
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
                if (this.IsQuickExit && keyEvent.KeyReleased(Keys.Escape))
                {
                    Exit();
                }
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
                this.DrawText(String.Format("{0:0.0} FPS", this.fps),(int)FPSPosition.X,(int)FPSPosition.Y);
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
            spriteBatch.Begin(this.SpriteSortMode, BlendState.NonPremultiplied);
#else
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, this.SpriteSortMode, SaveStateMode.None);
#endif
// TODO: When Main is fully deprecated (after LD), remove this block.
#pragma warning disable 612, 618
            this.currentState.Main(gameTime);
#pragma warning restore 612, 618
            this.currentState.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
