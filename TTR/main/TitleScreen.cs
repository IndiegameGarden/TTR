// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTR.gameobj;

namespace TTR.main
{
    /** title screen and main gamelet of TTR.
     */
    public class TitleScreen: Gamelet
    {
        SubtitleText testTxt;

        public TitleScreen(): 
            base(new TitleScreenState())
        {
            testTxt = new SubtitleText("TT1 title screen");
            Add(testTxt);
            
            GoLEffect gol = new GoLEffect("ttlogo-gol");
            gol.LayerDepth = 0f;
            gol.Position = new Vector2(0.6f, 0.4f);
            gol.Scale = 4.0f;
            gol.Rotate = 0.0f;
            gol.Add(new PeriodicPulsingBehavior(0.05f, 140f/60f/16f)); // 140 / 60 * 1/16
            Add(gol);
           
        }

        protected override void OnInit()
        {
            base.OnInit();
        }


        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            testTxt.Position = new Microsoft.Xna.Framework.Vector2(0.4f,0.8f);
            Screen.DebugText(PositionAbsolute, "Testing Title Screen");
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Parent.SetNextState(new LevelIntroState() );
        }
    }
}
