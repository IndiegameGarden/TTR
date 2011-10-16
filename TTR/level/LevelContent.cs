// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using TTMusicEngine;
using TTR.gameobj;

namespace TTR.level
{
    public abstract class LevelContent
    {
        protected Screen screen;
        protected SoundTrackEvent script;
        protected GameItem rootItem;

        public LevelContent(Screen screen)
        {
            this.screen = screen;
            rootItem = new GameItem();
        }

        /** initialize the level's data structure - after this first call, other Create...() methods are called */
        public virtual void Initialize()
        {
            //
        }

        /** return a tree with this level's sound/music script */
        public virtual SoundEvent CreateSoundScript()
        {
            return script;
        }

        /** TODO return a tree with this level's item and item-creation script, matching the music */
        public virtual GameItem CreateItemsScript()
        {
            return rootItem;
        }

        /** create the ship of this level - default ship here */
        public virtual Ship CreateShip()
        {
            Ship s = new Ship();
            KeyboardShipControl shipControl = new KeyboardShipControl();
            s.Add(shipControl);
            return s;
        }

        /** obtain the music's BPM for this level - used for time/beat conversion */
        public virtual double GetBPM()
        {
            return 0.0;
        }

        /** obtain reference to the main (beat) Sound track of the SoundScript or null if none exists - used to sync game's time to sound */
        public virtual SampleSoundEvent GetMainSoundTrack()
        {
            return null;
        }


    }

}
