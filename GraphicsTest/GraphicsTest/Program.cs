using System;

namespace Spiridios.GraphicsTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GraphicsTest game = new GraphicsTest())
            {
                game.Run();
            }
        }
    }
#endif
}

