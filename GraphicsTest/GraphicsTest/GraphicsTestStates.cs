using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Scene;
using System;

namespace Spiridios.GraphicsTest
{
    public class TestState : State
    {
        private Scene testMap;
        private Camera camera;


        public TestState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            testMap = new Scene(game);
            testMap.LoadTiledMap("TestMap.tmx");
            camera = new Camera();
            camera.Position = new Vector2(352, 272);
            testMap.Camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            testMap.Draw(game.SpriteBatch);
        }
    }
}
