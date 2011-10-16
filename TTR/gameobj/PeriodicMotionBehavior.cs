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
    public class PeriodicMotionBehavior : Gamelet
    {
        Vector2 direction;
        float frequency;

        public PeriodicMotionBehavior(Vector2 direction, float frequency)
            : base()
        {
            this.direction = direction;
            this.frequency = frequency;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            Parent.Position += (direction * (float)Math.Sin(2.0 * Math.PI * (double)frequency * SimTime));
        }
    }
}
