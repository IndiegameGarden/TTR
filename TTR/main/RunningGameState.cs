// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using TTMusicEngine;

namespace TTR.main
{
    public static class RunningGameState
    {
        // currently active MusicEngine, or null if none
        public static MusicEngine musicEngine;

        // current global game state
        public static TTRStateMachine StateMachine;

        // using Reach or HiDef XNA profile?
        public static bool IsXNAHiDef;

        // stats of player(s)
        public static Player player;

    }

}
