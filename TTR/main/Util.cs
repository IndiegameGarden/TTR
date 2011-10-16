// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTR.main
{
    public class Util
    {
        private static TextWriter logTw = null;

        /**
         * log msg to file, if Util.Logging = true
         */
        public static void Log(string s)
        {            
            if (logTw == null)
                logTw = new StreamWriter("log_ttr.txt");
            logTw.Write(s);
            logTw.Flush();
        }

    }
}
