// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TTengine;
using TTR.gameobj;
using TTR.main;

namespace TTR
{
    public class StarCloudsEffect : Gamelet
    {
        #region Constants

        /// The period of the parallax motion in the starfield.
        const float starsParallaxPeriod = 30f;

        /// The amplitude of the parallax motion in the starfield.
        const float starsParallaxAmplitude = 2048f;

        /// Persistent movement tracker, used to slightly parallax the stars.
        private double movement;

        /// The maximum amount of movement allowed per update.
        /// Any per-update movement values that exceed this will trigger a 
        /// starfield reset.
        const float maximumMovementPerUpdate = 128f;

        /// The background color of the starfield.
        static readonly Color backgroundColor = new Color(0, 0, 32);

        #endregion


        #region Gameplay Data

        /// The last/previous position, used for the parallax eff.
        private Vector2 lastPosition;

        #endregion


        #region Graphics Data

        private SpriteBatch spriteBatch;
        private Texture2D cloudsTexture;

        /// The eff used to draw the clouds.
        private Effect eff;

        /// The parameter on the cloud eff that receives the current position
        private EffectParameter cloudEffectPosition;

        #endregion


        #region Initialization Methods

        public StarCloudsEffect(): base()
        {            
        }

        protected override void OnInit()
        {
            Reset(Vector2.Zero);

            // load the cloud eff
            cloudsTexture = TTengineMaster.ActiveGame.Content.Load<Texture2D>("clouds");
            eff = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/Clouds");
            cloudEffectPosition = eff.Parameters["Position"];
            spriteBatch = new SpriteBatch(Screen.graphicsDevice);

            VertexShaderInit(eff);
        }

        /// Reset the stars and the parallax eff.
        /// <param name="position">The new origin point for the parallax eff.</param>
        public void Reset(Vector2 position)
        {
            // reset the position
            this.lastPosition = this.Position = position;
        }


        #endregion


        #region Update and Draw Methods


        protected override void OnUpdate(ref UpdateParams p)
        {

            // update the parallax movement
            // update the current position
            this.lastPosition = this.Position;
            movement += p.dt;
            Position = Vector2.Multiply(new Vector2(
                    (float)Math.Cos(movement / starsParallaxPeriod),
                    (float)Math.Sin(movement / starsParallaxPeriod)),
                    starsParallaxAmplitude);

        }

        protected override void OnDraw(ref DrawParams p)
        {
            // determine the movement vector of the stars
            // -- for the purposes of the parallax eff, 
            //    this is the opposite direction as the position movement.
            Vector2 movement = -1.0f * (Position - lastPosition);

            // draw the cloud texture
            cloudEffectPosition.SetValue(this.Position);
            //cloudEffectStretch.SetValue(1.0f/(1f + (float) Math.Sin(gameTime.TotalGameTime.TotalSeconds)) );
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, eff);
            spriteBatch.Draw(cloudsTexture, Screen.ScreenRectangle, null, Color.White, 0.0f,
                Vector2.Zero, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            // if we've moved too far, then reset, as the stars will be moving too fast
            if (movement.Length() > maximumMovementPerUpdate)
            {
                Reset(Position);
                return;
            }

        }


        #endregion


    }
}

