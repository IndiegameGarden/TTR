// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
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
     * Efflet to shake the entire screen e.g. for explosion effects
     */
    public class ScreenShake: Efflet
    {
        protected float amplx, amply, freq;

        public ScreenShake(float amplx, float amply, float freq): base(null)
        {
            this.amplx = amplx;
            this.amply = amply;
            this.freq = freq;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            float shakeAmplx = amplx * (float) Math.Sin(2*Math.PI*freq*SimTime);
            float shakeAmply = amplx * (float)Math.Sin(2 * Math.PI * (freq + 0.0238223) * SimTime);
            shakeAmplx += RandomMath.RandomBetween(-amplx, +amplx);
            shakeAmply += RandomMath.RandomBetween(-amply, +amply);
            //parentPars.position += new Vector2(shakeAmplx, shakeAmply);
            Position = new Vector2(shakeAmplx, shakeAmply);
        }

        public override void OnDrawEfflet(ref DrawParams p, Microsoft.Xna.Framework.Graphics.RenderTarget2D sourceBuffer)
        {
            Screen.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            Screen.spriteBatch.Draw(sourceBuffer, Vector2.Zero, Color.White);
            Screen.spriteBatch.Draw(sourceBuffer, Screen.ToPixels(Position ), Color.White);
            Screen.spriteBatch.End();
        }

    }
}
