// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTMusicEngine;
using TTR.gameobj;

namespace TTR.main
{
    /** 
     * this class plugs in to the TTMusicEngine SoundEvents. Its task is to "create", at the right moment in time in the music,
     * a GameItem which will appear on screen. Actual creation is done at init time of course. "create" here refers to
     * adding the GameItem to the render tree as a child of some parentItem.
     */
    public class ItemCreateSoundEvent: SoundEvent
    {      

        // set to true after the 'item' has been spawned
        protected bool renderDone = false;
        protected GameItem gameItem, parentForItem;

        /**
         * item is the GameItem to create upon rendering this event
         * parentForItem is the parent to (later) attach it to, during rendering if event triggers.
         */
        public ItemCreateSoundEvent(GameItem itemToAppear, GameItem parentForItem): base()
        {
            if (itemToAppear is Ball)
                _duration = (itemToAppear as Ball).GetTrajectoryDuration();
            else
                _duration = 0; 
            this.gameItem = itemToAppear;
            this.parentForItem = parentForItem;
        }

        /**
         * call this to attach this event to a parent SoundEvent in a
         * rendering tree. Uses a specific internal trick of 'negative time'
         * to accomodate for the duration of the event. (Climax is at end of event)
         */
        public void AttachTo(SoundEvent ev)
        {
            ev.AddEvent(-_duration,this);
        }
        
        public override bool Render(RenderParams rp, RenderCanvas c)
        {
            if (!Active) return false;
            if (rp.Time >= 0 && !renderDone )  // yes its my turn!
            {
                // spawn new item ...
                parentForItem.Add(gameItem);
                renderDone = true;
            }
            else if (rp.Time < 0)
            {
                // if time < 0 reset back the render flag eg for purpose of replay of this script.
                renderDone = false;
            }
            return true;
        }


    }
}
