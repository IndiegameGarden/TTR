// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTR.main;
using TTengine;
using TTengine.Core;

namespace TTR.gameobj
{
    public class MandelBackground: Spritelet
    {
        Effect eff;
        SpriteBatch spriteBatch;

        Vector2 juliaSeed = new Vector2(0.39f, -0.2f);
        Vector3 colorScale = new Vector3(4, 5, 6);
        float zoom = 6f;
        int iterations = 64;

        public MandelBackground(): base()
        {
            Position = new Vector2(0.25f, 0.00f);
        }

        protected override void OnInit()
        {
            Alpha = 0.4f;
            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
            eff = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/MandelBackground");
            Texture = new Texture2D(Screen.graphicsDevice, Screen.WidthPixels, Screen.HeightPixels);
            eff.Parameters["Aspect"].SetValue(1.0f/Screen.AspectRatio);
            VertexShaderInit(eff);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            zoom /= (1+ p.dt * 0.2f);
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) Position -= new Vector2(0f, 0.001f);
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) Position += new Vector2(0f, 0.001f);
        }

        protected override void OnDraw(ref DrawParams p)
        {
            eff.Parameters["Pan"].SetValue(Position);
            eff.Parameters["Zoom"].SetValue(zoom);
            eff.Parameters["Iterations"].SetValue(iterations);
            eff.Parameters["JuliaSeed"].SetValue(juliaSeed);
            eff.Parameters["Alpha"].SetValue(Alpha);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, eff);
            spriteBatch.Draw(Texture, Screen.ScreenRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }
    }
}
