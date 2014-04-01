// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTR.main;
using TTengine.Core;

namespace TTR.gameobj
{
    /**
     * deform/bulge out the game screen to accompany a booming (sound) eff.
     */
    public class BoomDepthEffect : Efflet
    {
        EffectParameter depthParam, centerParam;
        public float Ampl = 0.1f;

        public BoomDepthEffect()
            : base("Effects/TextureStretch")
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            depthParam = effect.Parameters["Depth"];
            centerParam = effect.Parameters["Center"];
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            // function: quick boom bulge out then slower decay towards zero again.
            float t = 1f*SimTime % (140f/60f);
            float d = - Ampl * t * (float)Math.Exp(-8.0f * (double)t);
            depthParam.SetValue( new Vector2(d,d) );
        }

    }
}
