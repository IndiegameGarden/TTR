// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTR.main;
using TTengine;

namespace TTR.gameobj
{
    /**
     * deform/bulge out the game screen to accompany a booming (sound) eff.
     */
    public class BoomDepthEffect : Efflet
    {
        EffectParameter depthParam, centerParam;
        float ampl = 0.1f;

        public BoomDepthEffect()
            : base("Effects/TextureStretch")
        {
        }

        /// Amplitude of the eff
        public float Ampl
        {
            get { return ampl; }
            set { ampl = value; }
        }

        protected override void OnInit()
        {
            base.OnInit();

            depthParam = effect.Parameters["Depth"];
            centerParam = effect.Parameters["Center"];

            //LoadTexture("galaxy_512");
            //LoadTexture("grid");

            //centerParam.SetValue(new Vector2(0.5f, 0.1f)); // test

            VertexShaderInit(effect);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            /*
            float f = 0.3473f;
            float xd = ampl * (float)Math.Sin(2 * Math.PI * (double)myPars.simTime * f);
            float yd = ampl     * (float)Math.Sin(2 * Math.PI * (double)myPars.simTime * f);
             */
            // function: quick boom bulge out then slower decay towards zero again.
            float t = 1f*SimTime % (140f/60f);
            float d = - Ampl * t * (float)Math.Exp(-8.0f * (double)t);
            depthParam.SetValue( new Vector2(d,d) );
        }

        public override void OnDrawEfflet(ref DrawParams p, RenderTarget2D sourceBuffer)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(sourceBuffer, Screen.ScreenRectangle, Color.White);
            spriteBatch.End();
        }

    }
}
