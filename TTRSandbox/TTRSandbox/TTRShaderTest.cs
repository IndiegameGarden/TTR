// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TTR
{
    class TTRShaderTest: Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Effect eff;
        Texture2D texture;
        SpriteFont font1;
        Rectangle screenRectangle;
        Vector2 posVec = Vector2.Zero;

        // clouds.fx
        const float starsParallaxPeriod = 30f;
        const float starsParallaxAmplitude = 2048f;
        private double movement;
        const float maximumMovementPerUpdate = 128f;


        public TTRShaderTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {           
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            eff = Content.Load<Effect>("Effects/Clouds");
            texture = new Texture2D(graphics.GraphicsDevice,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height,
                false, SurfaceFormat.Color);
            texture = Content.Load<Texture2D>("clouds");
            screenRectangle = new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);

            Viewport viewport = GraphicsDevice.Viewport;

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            eff.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            // for Clouds eff
            movement += gameTime.ElapsedGameTime.TotalSeconds;
            posVec = Vector2.Multiply(new Vector2(
                    (float)Math.Cos(movement / starsParallaxPeriod),
                    (float)Math.Sin(movement / starsParallaxPeriod)),
                    starsParallaxAmplitude);
            eff.Parameters["Position"].SetValue(posVec);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //float aspectRatio = (float)GraphicsDevice.Viewport.Height / (float)GraphicsDevice.Viewport.Width;
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, eff);
            eff.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(texture, screenRectangle, null, Color.White);

            spriteBatch.End();

            spriteBatch.Begin();
            String txt = "Time: " + gameTime.TotalGameTime.TotalSeconds;
            spriteBatch.DrawString(font1, txt, new Vector2(10, 10), Color.White,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            txt = "Pos: " + posVec;
            spriteBatch.DrawString(font1, txt, new Vector2(10, 20), Color.White,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    
    }
}
