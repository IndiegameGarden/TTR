// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTR.main;
using TTengine;
using TTengine.Core;

namespace TTR.gameobj
{
    /**
     * A shader vortext eff. A vortex eff is any shader eff having a Position, to set the focal point of the eff ; 
     * a VortexVelocity, to set the speed of the vortex. Use NoiseLevel to set the level of noise for the eff (how noise is
     * used, is eff specific!)
     * Use Velocity and Acceleration to manipulate the focal point as with any Gamelet.
     */
    public class VortexEffect: Spritelet
    {
        /**
         * the average velocity of the clouds motion
         */
        public float VortexVelocity {  get { return cloudsVelocity; } set { cloudsVelocity = value; } }
        public float NoiseLevel { get { return noiseLevel; } set { noiseLevel = value; } }
        protected float cloudsVelocity = 0.03f;
        protected float noiseLevel = 0f;
        protected float maxAlpha = float.NaN;

        // fx related
        protected Effect effect;
        protected EffectParameter fxPosition, fxVelocity, fxTime, fxNoiseLevel, fxAlpha;
        protected Texture2D vortexTexture;
        protected SpriteBatch spriteBatch;

        public VortexEffect( string fxName)
            : base()
        {
            LoadContent(fxName, null);
        }

        public VortexEffect( string fxName, string textureName)
            : base()
        {
            LoadContent(fxName, textureName);
        }

        protected void LoadContent(string fxName, string textureName)
        {
            if(textureName != null)
                vortexTexture = TTengineMaster.ActiveGame.Content.Load<Texture2D>(textureName);
            effect = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/" + fxName);

        }

        protected override void OnInit()
        {
            fxPosition = effect.Parameters["Position"];
            fxVelocity = effect.Parameters["Velocity"];
            fxTime = effect.Parameters["Time"];
            fxAlpha = effect.Parameters["Alpha"];
            fxNoiseLevel = effect.Parameters["NoiseLevel"];
            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            // set Max expected Alpha for the first time when called.
            if (float.IsNaN(maxAlpha))
                maxAlpha = Alpha;

            // TODO fade in behaviour
            if (SimTime < 1.0f)
            {
                Alpha = MathHelper.Clamp(SimTime, 0f, maxAlpha);
            }

            // fade out behaviour
            if (Duration > 0)
            {
                if (SimTime > Duration - 5.0f)
                {
                    Alpha = MathHelper.Clamp((Duration - SimTime) / 5f, 0f, maxAlpha);
                    
                }
            }
        }

        protected override void OnDraw(ref DrawParams p)
        {
            // create a rectangle representing the screen dimensions of the starfield
            Rectangle drawRect = Screen.ScreenRectangle;
            Vector2 pos = PositionAbsolute;
            fxPosition.SetValue(new Vector2(pos.X / Screen.AspectRatio, pos.Y));
            fxVelocity.SetValue(this.VortexVelocity);
            fxTime.SetValue( SimTime );
            fxNoiseLevel.SetValue( noiseLevel );
            fxAlpha.SetValue(Alpha);

            if (vortexTexture != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, effect);
                spriteBatch.Draw(vortexTexture, drawRect, null, DrawColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.992f);
                spriteBatch.End();
            }
        }


    }
}
