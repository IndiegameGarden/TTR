// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine;
using TTR.main;

namespace TTR.gameobj
{
    public class Score : Gamelet
    {
        public float scoreCurrent = 100f;
        public float ScoreTarget = 0f;
        public float ScoreSpeed = 200.0f;

        SpriteFont spriteFont;

        public Score(): base()
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>("m41_lovebit");
            Position = new Vector2();
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            // copy scope
            ScoreTarget = RunningGameState.player.Score;

            // move nrg level towards the target
            if (scoreCurrent < ScoreTarget)
            {
                scoreCurrent += ScoreSpeed * p.dt;
                if (scoreCurrent > ScoreTarget)
                    scoreCurrent = ScoreTarget;
            }
            if (scoreCurrent > ScoreTarget)
            {
                scoreCurrent -= ScoreSpeed * p.dt;
                if (scoreCurrent < ScoreTarget)
                    scoreCurrent = ScoreTarget;
            }

        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = Screen.ToPixels(PositionAbsolute);

            // plot text 
            Screen.spriteBatch.DrawString(spriteFont, String.Format("{0}", Math.Round(scoreCurrent)), pos, Color.White);
        }

    }
}
