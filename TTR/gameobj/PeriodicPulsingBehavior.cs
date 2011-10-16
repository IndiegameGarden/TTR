// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine;
using TTR.main;

namespace TTR.gameobj
{
    /**
     * let an item periodically move back and forth (eg sine-wave based)
     */
    public class PeriodicPulsingBehavior : Gamelet
    {
        float ampl;
        float frequency;

        public PeriodicPulsingBehavior(float ampl, float frequency)
            : base()
        {
            this.ampl = ampl;
            this.frequency = frequency;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            Parent.ScaleModifier = 1.0f + ampl * (float)Math.Sin(MathHelper.TwoPi * (double)frequency * SimTime);
        }
    }
}
