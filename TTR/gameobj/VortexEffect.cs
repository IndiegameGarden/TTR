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
     * A HLSL shader vortex effect. A vortex eff is any shader eff having a Position, to set the focal point of the eff ; 
     * and a VortexVelocity, to set the speed of the vortex. Use NoiseLevel to set the level of noise for the eff (how noise is
     * used, is eff specific!)
     * Use Velocity and Acceleration to manipulate the focal point as with any Gamelet.
     */
    public class VortexEffect: EffectSpritelet
    {
        /**
         * the average velocity of the vortex - how used, is .fx file specific
         */
        public float VortexVelocity {  get { return vortexVelocity; } set { vortexVelocity = value; } }

        /**
         * the level of noise - how noise is used is .fx file specific
         */
        public float NoiseLevel { get { return noiseLevel; } set { noiseLevel = value; } }
        
        protected float noiseLevel = 0f;
        protected float vortexVelocity = 0.03f;
        protected float maxAlpha = float.NaN;

        // HLSL fx related
        protected EffectParameter fxPosition, fxVelocity, fxTime, fxNoiseLevel, fxAlpha;

        public VortexEffect(string fxName, string textureName)
            : base(textureName,fxName)
        {            
        }

        protected override void OnInit()
        {
            base.OnInit();

            Center = Vector2.Zero;

            // fx parameters
            fxPosition = eff.Parameters["Position"];
            fxVelocity = eff.Parameters["Velocity"];
            fxTime = eff.Parameters["Time"];
            fxAlpha = eff.Parameters["Alpha"];
            fxNoiseLevel = eff.Parameters["NoiseLevel"];

        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

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
            drawRect.Width = (int) (ScaleAbsolute * (float)drawRect.Width);
            drawRect.Height = (int)(ScaleAbsolute * (float)drawRect.Height);

            Vector2 pos = PositionAbsolute;
            fxPosition.SetValue(new Vector2(pos.X / Screen.AspectRatio, pos.Y));
            fxVelocity.SetValue(VortexVelocity);
            fxTime.SetValue( SimTime );
            fxNoiseLevel.SetValue( noiseLevel );
            fxAlpha.SetValue(Alpha);

            spriteBatch.Begin(spriteSortMode, blendState, null, null, null, eff);
            spriteBatch.Draw(Texture, drawRect, null, DrawColor,
                   RotateAbsolute, DrawCenter, SpriteEffects.None, LayerDepth);
            spriteBatch.End();

        }


    }
}
