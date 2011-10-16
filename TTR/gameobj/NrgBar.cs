// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTengine;
using TTR.main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTR.gameobj
{
    class NrgBar : Spritelet
    {
        float curNrgValue;
        float nrgValueTarget;
        float nrgCatchupSpeed = 50.0f;
        SpriteFont spriteFont;
        Color textColor = Color.White;

        public NrgBar()
            : base("nrg-bar")
        {
            Position = new Vector2(0.2f, 0.0f);
            this.Alpha = 0.05f;
            this.LayerDepth = 0f;
            curNrgValue = 0f;
            nrgValueTarget = 50f;
        }

        protected override void OnInit()
        {
            base.OnInit();
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>("m41_lovebit");
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            // copy target
            nrgValueTarget = RunningGameState.player.NRG;

            // move nrg level towards the target
            if (curNrgValue < nrgValueTarget)
            {
                curNrgValue += nrgCatchupSpeed * p.dt;
                if (curNrgValue > nrgValueTarget)
                    curNrgValue = nrgValueTarget;
            }
            if (curNrgValue > nrgValueTarget)
            {
                curNrgValue -= nrgCatchupSpeed * p.dt;
                if (curNrgValue < nrgValueTarget)
                    curNrgValue = nrgValueTarget;
            }

            // draw color
            if (curNrgValue > 120f || curNrgValue < 20f)
            {
                if (SimTime % 0.6f > 0.2f)
                    textColor = Color.Red;
                else
                    textColor = Color.Black;
            }
            else
            {
                textColor = Color.White;
            }


        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = Screen.ToPixels(DrawPosition );
            int width = (int) Math.Round(131f + 480f/100f * curNrgValue );
            if (width > Texture.Width) width = Texture.Width;
            //Rectangle rect = new Rectangle( posX , posY , posX + width, posY + Texture.Height);
            Rectangle srcRect = new Rectangle(0, 0, width, Texture.Height);
            Screen.spriteBatch.Draw(Texture, pos, srcRect, DrawColor,
                    this.RotateAbsolute, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);

            // plot text percentage
            Vector2 tpos = pos + new Vector2(width, Texture.Height / 2.0f - 10.0f) ;
            Screen.spriteBatch.DrawString(spriteFont, String.Format("{0,3}%", Math.Round(curNrgValue)), tpos, textColor);
        }

    }

}
