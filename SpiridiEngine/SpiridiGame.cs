using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Spiridios.SpiridiEngine
{
    public abstract class SpiridiGame : Microsoft.Xna.Framework.Game
    {
        private static Random random = new Random();

        private Color clearColor = new Color(0xff, 0x80, 0xc0);
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        protected ImageManager imageManager;
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
            this.imageManager = new ImageManager(Content);
            Actor.imageManager = this.imageManager;

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

        public static String NormalizeFilename(String fileName)
        {
            int dotPos = fileName.LastIndexOf('.');
            if (dotPos < 0)
            {
                return fileName;
            }
            else
            {
                return fileName.Substring(0, dotPos);
            }
            //The following is not supported by JSIL.
            //return System.IO.Path.ChangeExtension(fileName, null);
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
