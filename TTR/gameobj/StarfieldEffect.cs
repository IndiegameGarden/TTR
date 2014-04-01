// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TTR.gameobj;
using TTR.main;
using TTengine.Core;

namespace TTR
{
    /// <summary>
    /// The starfield that renders behind the game, including a parallax eff.
    /// </summary>
    public class StarfieldEffect : Spritelet
    {
        #region Constants

        /// The period of the parallax motion in the starfield.
        const float starsParallaxPeriod = 30f;

        /// The amplitude of the parallax motion in the starfield.
        const float starsParallaxAmplitude = 2048f;

        /// The number of stars in the starfield.
        const int numberOfStars = 128;

        /// The number of layers in the starfield.
        const int numberOfLayers = 5;

        /// The colors for each layer of stars.
        static readonly Color[] layerColors = new Color[numberOfLayers] 
            {                 
                new Color(255, 255, 255, 160), 
                new Color(255, 255, 255, 128), 
                new Color(255, 255, 255, 96), 
                new Color(255, 255, 255, 64), 
                new Color(255, 255, 255, 32) 
            };

        /// The movement factor for each layer of stars, used in the parallax eff.
        static readonly float[] movementFactors = new float[numberOfLayers]
            {
                0.6f, 0.5f, 0.4f, 0.3f, 0.2f
            };

        /// The maximum amount of movement allowed per update.
        /// Any per-update movement values that exceed this will trigger a 
        /// starfield reset.
        const float maximumMovementPerUpdate = 128f;

        /// The size of each star, in pixels.
        const int starSize = 1;

        #endregion


        #region Vars

        /// Persistent movement tracker, used to slightly parallax the stars.
        private double movement;

        private float speed;

        /// The last/previous position, used for the parallax eff.
        private Vector2 lastPosition;

        /// The stars in the starfield.
        private Vector2[] stars;

        #endregion


        #region Graphics Data

        //private SpriteBatch spriteBatch;
        private Texture2D starTexture;

        #endregion

        #region Initialization Methods

        public StarfieldEffect( float speed)
            : base()
        {
            this.speed = speed;
        }

        protected override void OnInit()
        {
            LayerDepth = 0f;
            stars = new Vector2[numberOfStars];
            Reset(Vector2.Zero);

            // create the star texture
            starTexture = new Texture2D(Screen.graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            starTexture.SetData<Color>(new Color[] { Color.Ivory });
        }

        /// Reset the stars and the parallax eff.
        /// <param name="position">The new origin point for the parallax eff.</param>
        public void Reset(Vector2 position)
        {
            // recreate the stars
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i] = new Vector2(RandomMath.Random.Next(0, Screen.WidthPixels),
                    RandomMath.Random.Next(0, Screen.HeightPixels));
            }

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
            movement += speed * p.gameTime.ElapsedGameTime.TotalSeconds;
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

            // create a rectangle representing the screen dimensions of the starfield
            Rectangle starfieldRectangle = Screen.ScreenRectangle;

            Vector2 pos = Parent.PositionAbsolute;
            int posOffsetX = (int)Screen.ToPixels(pos.X);
            int posOffsetY = (int)Screen.ToPixels(pos.Y);

            // if we've moved too far, then reset, as the stars will be moving too fast
            if (movement.Length() > maximumMovementPerUpdate)
            {
                Reset(Position);
                return;
            }

            // draw all of the stars
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            for (int i = 0; i < stars.Length; i++)
            {
                // move the star based on the depth
                int depth = i % movementFactors.Length;
                stars[i] += movement * movementFactors[depth];

                // wrap the stars around
                if (stars[i].X < starfieldRectangle.X)
                {
                    stars[i].X = starfieldRectangle.X + starfieldRectangle.Width;
                    stars[i].Y = starfieldRectangle.Y +
                        RandomMath.Random.Next(starfieldRectangle.Height);
                }
                if (stars[i].X > (starfieldRectangle.X + starfieldRectangle.Width))
                {
                    stars[i].X = starfieldRectangle.X;
                    stars[i].Y = starfieldRectangle.Y +
                        RandomMath.Random.Next(starfieldRectangle.Height);
                }
                if (stars[i].Y < starfieldRectangle.Y)
                {
                    stars[i].X = starfieldRectangle.X +
                        RandomMath.Random.Next(starfieldRectangle.Width);
                    stars[i].Y = starfieldRectangle.Y + starfieldRectangle.Height;
                }
                if (stars[i].Y >
                    (starfieldRectangle.Y + Screen.HeightPixels))
                {
                    stars[i].X = starfieldRectangle.X +
                        RandomMath.Random.Next(starfieldRectangle.Width);
                    stars[i].Y = starfieldRectangle.Y;
                }

                // draw the star
                Screen.spriteBatch.Draw(starTexture,
                    new Rectangle((int)stars[i].X + posOffsetX, (int)stars[i].Y + posOffsetY, starSize, starSize),
                    null, layerColors[depth], Rotate, Vector2.Zero, SpriteEffects.None, LayerDepth);
            }
            //spriteBatch.End();
        }


        #endregion


    }
}

