// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Util;
using TTR.level;

namespace TTR.main
{
    public class TitleScreenState : State    { }
    public class LevelIntroState : State { }
    public class LevelState : State { }

    /**
     * simple state machine to select between the game screens (title, options, loading, level, etc)
     * Children of this gamelet will be selected/executed depending on current state of this Gamelet.
     */
    public class TTRStateMachine: Gamelet
    {
        LevelIntro levelIntro;

        public TTRStateMachine()
        {
            // add child gamelets that are state-dependent
            Add(new TitleScreen());
            levelIntro = new LevelIntro();
            Add(levelIntro);
        }

        protected override void OnInit()
        {
            base.OnInit();
            SetNextState(new TitleScreenState());
            RunningGameState.player = new Player();

        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
        }
    }

    
}
