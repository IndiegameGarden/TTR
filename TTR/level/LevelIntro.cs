// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;


using TTengine.Core;
using TTR.gameobj;
using TTR.main;

namespace TTR.level
{
    public class LevelIntro: Gamelet
    {
        SubtitleText testTxt;
        bool loadingStarted = false;
        bool loadingReady = false;

        public int LevelNumber = 1;
        public String Name = "DefaultName";

        public LevelIntro()
            : base(new LevelIntroState())
        {
        }

        protected override void OnInit()
        {
            testTxt = new SubtitleText("TTR LevelIntro");
            this.Add(testTxt);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            if (!loadingStarted)
            {
                loadingStarted = true;
                loadingReady = false;
                Thread oThread = new Thread(new ThreadStart(this.LoadLevel));
                oThread.Priority = ThreadPriority.Normal;
                oThread.Start();
            }

            if (loadingReady)
            {
                Parent.SetNextState(new LevelState());
                loadingReady = false;
            }
        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            testTxt.Position = new Microsoft.Xna.Framework.Vector2(0.6f, 0.9f);
            Screen.DebugText(PositionAbsolute, "Testing LevelIntro");
        }

        /// level loading executes in a separate thread. Entry point here.
        public void LoadLevel()
        {
            Level level = LevelSelect.CreateLevel(LevelNumber);
            Parent.Add(level);
            TTengineMaster.Initialize(level);
            loadingReady = true;
        }

    }
}
