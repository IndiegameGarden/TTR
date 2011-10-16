// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
using System;

namespace TTMusicEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the test application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EngineTest game = new EngineTest())
            {
                game.Run();
            }

        }

    }
}

