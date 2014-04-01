// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTR.level
{
    class LevelSelect
    {
        public static Level CreateLevel(int levelNr)
        {
            Level level = null;
            switch(levelNr)
            {
                case 1:
                    level = new LevelTest();
                    break;
                case 2:
                    level = new LevelSpaceExplorersSublime();
                    break;
            }
            return level;
        }
    }
}
