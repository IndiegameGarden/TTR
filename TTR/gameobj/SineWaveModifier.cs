// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTR.main;

namespace TTR.gameobj
{

    public delegate void ModifyAction(float value);

    /**
     * let an item periodically move back and forth (eg sine-wave based)
     */
    public class SineWaveModifier : Gamelet
    {
        float ampl, frequency, offset;
        ModifyAction action;

        public SineWaveModifier(ModifyAction action, float ampl, float frequency, float offset)
            : base()
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = offset;
            this.action = action;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            //Parent.ScaleModifier = 
            float val = offset + ampl * (float)Math.Sin(MathHelper.TwoPi * (double)frequency * SimTime);
            action(val);
        }
    }
}
