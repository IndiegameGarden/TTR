// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using TTMusicEngine;
using TTMusicEngine.Soundevents;
using TTengine.Core;
using TTR.main;
using TTR.gameobj;

namespace TTR.level
{
    /// Generic item that plays the music associated to a level. 
    public class LevelMusic : Gamelet
    {
        RenderCanvas musicCanvas;
        RenderParams musicRp;
        SoundTrackEvent musicScript;
        SampleSoundEvent mainSoundTrack;
        float lastMusicEngineUpdTime = 0;

        /**
         * create the item that is able to play the level's music based on the music script.
         * If a mainSoundTrack=null is passed, no time syncing to a main sound track is used.
         */
        public LevelMusic( SoundTrackEvent musicScript, SampleSoundEvent mainSoundTrack)
            : base()
        {
            musicRp = new RenderParams();
            musicRp.RenderAheadTime = 15.0;
            this.musicScript = musicScript;
            this.mainSoundTrack = mainSoundTrack;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            // detect music render time rp.Time from the main beat track, if music plays
            double t = 0;
            if (mainSoundTrack != null)
                t = mainSoundTrack.CurrentPlayTime;
            if (t == 0)
            {
                musicRp.Time = p.gameTime.TotalGameTime.TotalSeconds;
            }
            else
            {
                musicRp.Time = t;
            }

            // music rendering - also apply a time rewind
            //musicRp.Time = p.gameTime.TotalGameTime.TotalSeconds ; //- musicStartTime - musicRewindTime ;
            //musicRp.Time -= musicRewindTime;
            if (RunningGameState.musicEngine != null)
            {
                musicCanvas = RunningGameState.musicEngine.Render(musicScript, musicRp);
                // music engine update - call once per frame only on average
                if (p.simTime >= lastMusicEngineUpdTime + 0.1667f)
                {
                    RunningGameState.musicEngine.Update();
                    lastMusicEngineUpdTime += 0.1667f;
                }

            }
            else
            {
                // fake the musicengine
                musicScript.Render(musicRp, new RenderCanvas() );
            }
            
        }

        protected override void OnDraw(ref DrawParams p)
        {
            string msg = "rp.Time: " + Math.Round(musicRp.Time, 3);
            Screen.DebugText(0.05f, 0.025f, msg);

            // draw time
            msg = "Beat: " + Math.Round ( Beat.TimeToBeat( musicRp.Time, musicScript.BPM) , 2) ;
            Screen.DebugText(0.25f, 0.025f, msg);

            // draw music track pos's
            List<SoundEvent> aT = musicScript.Children;
            for (int i = 0; i < aT.Count; i++)
            {
                if (aT[i] is SampleSoundEvent)
                {
                    SampleSoundEvent e = (SampleSoundEvent)aT[i];
                    if (e.LastRenderParams != null)
                    {
                        msg = i + "dMs: " + Math.Round(1000.0 * (e.CurrentPlayTime - musicRp.Time));
                        Screen.DebugText(0.75f, 0.02f + 0.025f * i, msg);
                    }
                }
            }
        }


    }
}
