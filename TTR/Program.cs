// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
using System;
using Microsoft.Xna.Framework;

namespace TTR
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new TTRGame())
            {
                game.Run();
            }
        }
    }
#endif
}

