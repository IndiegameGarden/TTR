// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using TTMusicEngine;
using TTR.gameobj;
using TTR.level;
using TTR.main;
using TTengine.Core;

namespace TTR
{
    public abstract class Level : Gamelet
    {
        public String Name = "Default";
        protected SoundTrackEvent musicScript ;
        protected Ship ship ;
        protected Gamelet statsPanel;

        /**
         * create a new level
         */
        public Level(String levelName)
            : base(new LevelState())
        {            
            Name = levelName;
        }

        protected override void OnInit()
        {
            CreateContent();
            CreateShip();
            CreateStats();
            CreateMusicScript();
            if (musicScript != null && musicScript.Children.Count > 0)
                this.Add( new LevelMusic(musicScript, musicScript.Children[0] as SampleSoundEvent ) );
            this.Add(ship);
        }

        /** create any level content before the music-script or before the ship. For example
         *  Efflets that operate all the time
         */
        public abstract void CreateContent();

        /** create this level's sound/music script. This method should add content into
         * this.musicScript. If a main audio track (eg beat) is present, it
         * should be the first child of musicScript.
         */
        public abstract void CreateMusicScript();

        /** create the ship of this level
         * can be overridden to make other ship types or add behaviours
         */
        public virtual void CreateShip()
        {
            ship  = new Ship();
            KeyboardShipControl shipControl = new KeyboardShipControl();
            ship.Add(shipControl);
            ship.player = RunningGameState.player;
        }

        public virtual void CreateStats()
        {
            statsPanel = new Gamelet();
            this.Add(statsPanel);
            NrgBar nrgbar = new NrgBar();
            statsPanel.Add(nrgbar);
            Score score = new Score();
            statsPanel.Add(score);
        }

        public virtual LevelIntro CreateLevelIntro()
        {
            LevelIntro l = new LevelIntro();
            l.Name = Name;
            return l;
        }


    }
}
