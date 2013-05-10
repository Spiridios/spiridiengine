using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Scene;

namespace Spiridios.GraphicsTest
{
    public class TestState : State
    {
        private Scene testMap;
        private Camera camera;
        private Image staticImage;
        private Vector2 staticImagePosition = new Vector2(64, 64);
        private Rectangle staticImageRectangle = new Rectangle(128,128,32,32);
        private AnimatedImage animatedImage;
        private Vector2 animatedImagePosition = new Vector2(64, 96);
        private SubsetImage subsetImage;
        private Vector2 subsetImagePosition = new Vector2(64, 128);

        private List<Image> mosaicImages = new List<Image>();
        private TileMapImage mosaicImage;
        private Vector2 mosaicImagePosition = new Vector2(64, 192);
        private Rectangle mosaicImageRectangle= new Rectangle(192, 192, 32, 32);

        public TestState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            game.ImageManager.AddImage("StaticImage", "StaticImage.png");
            game.ImageManager.AddImage("AnimatedImage", "AnimatedImage.png");
            game.ImageManager.AddImage("LR", "LR.png");
            game.ImageManager.AddImage("LL", "LL.png");
            game.ImageManager.AddImage("UL", "UL.png");
            game.ImageManager.AddImage("UR", "UR.png");

            mosaicImages.Add(new TextureImage("LR"));
            mosaicImages.Add(new TextureImage("LL"));
            mosaicImages.Add(new TextureImage("UR"));
            mosaicImages.Add(new TextureImage("UL"));
            mosaicImage = new TileMapImage(mosaicImages, 32, 32, 2, 2);


            staticImage = new TextureImage("StaticImage");
            staticImage.Origin = new Vector2(24, 24);

            animatedImage = new AnimatedImage("AnimatedImage.xml");
            animatedImage.Origin = new Vector2(24, 24);

            subsetImage = new SubsetImage("AnimatedImage", new Rectangle(32,32,32,32));
            subsetImage.Origin = new Vector2(24, 24);


            testMap = new Scene(game);
            testMap.LoadTiledMap("TestMap.tmx");
            camera = new Camera();
            camera.Position = new Vector2(352, 272);
            testMap.Camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animatedImage.Update(gameTime.ElapsedGameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            testMap.Draw(game.SpriteBatch);

            staticImage.Draw(game.SpriteBatch, staticImagePosition);
            staticImage.Draw(game.SpriteBatch, camera.TranslatePoint(staticImagePosition));
            staticImage.Draw(game.SpriteBatch, camera.TranslateRectangle(staticImageRectangle));

            animatedImage.Draw(game.SpriteBatch, animatedImagePosition);
            animatedImage.Draw(game.SpriteBatch, camera.TranslatePoint(animatedImagePosition));

            subsetImage.Draw(game.SpriteBatch, subsetImagePosition);
            subsetImage.Draw(game.SpriteBatch, camera.TranslatePoint(subsetImagePosition));

            mosaicImage.Draw(game.SpriteBatch, camera.TranslatePoint(mosaicImagePosition));
            mosaicImage.Draw(game.SpriteBatch, camera.TranslateRectangle(mosaicImageRectangle));
        }
    }
}
