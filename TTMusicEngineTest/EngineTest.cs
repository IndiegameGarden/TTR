// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using TTMusicEngine;

namespace TTMusicEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class EngineTest : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spBatch;
        SpriteFont spFont;
        MusicEngine musicEngine;
        SoundEvent soundScript;

        public EngineTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            musicEngine = MusicEngine.GetInstance();
            musicEngine.AudioPath = "..\\audio\\test";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spBatch = new SpriteBatch(GraphicsDevice);

            // font
            spFont = Content.Load<SpriteFont>(@"Default1");

            // -------------------------------------
            // Select which 'unit' tests to execute.
            // -------------------------------------
            EngineTestContent test = new EngineTestContent();            
            soundScript = test.Test_Script1();
            //soundScript = test.Test_MorphingSoundEvent();
            //soundScript = test.Test_Repeat();
            //soundScript = test.Test_RepeatForSampleSoundEvents();
            //soundScript = test.Test_OverlapInTime();
            //soundScript = test.Test_Speed();
            //soundScript = test.Test_OscFrequency();
            //soundScript = test.Test_TwoOscillators(); // BROKEN

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            RenderParams rp = new RenderParams();
            rp.Time = gameTime.TotalGameTime.TotalSeconds;
            musicEngine.Render(soundScript,rp);

            // exit if all events done
            if (rp.Time > soundScript.Duration)
                this.Exit();

            // call base update
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spBatch.Begin();
            string msg = "Time: " + Math.Round(gameTime.TotalGameTime.TotalSeconds,3);
            spBatch.DrawString(spFont, msg, new Vector2(70.0f, 50.0f), Color.AntiqueWhite);
            spBatch.End();

            // call parent class draw
            base.Draw(gameTime);
        }
    }
}
