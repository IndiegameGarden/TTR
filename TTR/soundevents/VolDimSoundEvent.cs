// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTMusicEngine;
using TTMusicEngine.Soundevents;
using TTR.gameobj;
using TTR.main;
using TTengine.Core;

namespace TTR.soundevents
{
    /**
     * a temporarily volume dimming sound-event with specific timing parameters. Will listen
     * to a Gamelet for determining (via events) how and if volume should be dimmed.
     * For example, if collission detected do not dim volume.
     */
    public class VolDimSoundEvent : SoundEvent
    {
        public VolDimSoundEvent(Gamelet item, double timeDuration)
        {
            // specify a dimming envelope Signal and add to this as a child.
            Signal sig = new Signal(new List<double>() { 0, 1, 0.020, 0, timeDuration + 0.050, 0, timeDuration + 0.1, 1 });
            SignalSoundEvent se = new SignalSoundEvent(SignalSoundEvent.Modifier.AMPLITUDE, sig);
            this.AddEvent(0, se);

            // use eventing to add me as listener
            item.OnCollisionEvent += new GameletEventHandler(OnBallCollided);
        }

        /**
         * called upon collision event of my associated NRGCharge (or Ball or ... other)
         */
        public void OnBallCollided(Gamelet item, GameletEventArgs e)
        {
            if (e.otherItem is Ship)
            {
                // switch off my dimming behaviour.
                this.Active = false;
            }
        }

    }
}
