// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
using System;
using Microsoft.Xna.Framework;

namespace TTR
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new TTRSandbox())
            //using (Game game = new TTRShaderTest())
            {
                game.Run();
            }
        }
    }
#endif
}

