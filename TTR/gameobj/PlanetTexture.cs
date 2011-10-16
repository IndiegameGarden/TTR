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
    public class PlanetTexture : Spritelet
    {
        Effect effect;
        EffectParameter depthParam, centerParam;
        float ampl = 0.5f;
        SpriteBatch spriteBatch;

        public PlanetTexture()
            : base("earthmap1024")
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

            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
            effect = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/SphereProj_R");
            depthParam = effect.Parameters["Depth"];
            centerParam = effect.Parameters["Center"];
            //LoadTexture("galaxy_512");
            //LoadTexture("grid");

            //centerParam.SetValue(new Vector2(0.5f, 0.1f)); // test
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            
            depthParam.SetValue( ampl * (float) Math.Sin((float) MathHelper.TwoPi * 0.3f * SimTime));
        }

        protected override void OnDraw(ref DrawParams p)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque, null, null, null, effect);
            Vector2 ctr = Screen.ToPixelsNS(Center.X * width, Center.Y * height);
            Vector2 pos = Screen.ToPixels(DrawPosition); //DrawPosition);
            spriteBatch.Draw(Texture, pos, null, DrawColor,
                   this.RotateAbsolute, ctr, this.DrawScale, SpriteEffects.None, LayerDepth);
            spriteBatch.End();
        }

    }
}
