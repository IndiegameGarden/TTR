// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;
using TTR.main;

namespace TTR.gameobj
{
    /**
     * Displays a subtitle on screen for a specified time (no rotation or scale at this moment)
     */
    public class SubtitleText : Spritelet
    {
        protected string text;
        protected SpriteFont spriteFont;

        public SubtitleText( string text)
            : base()
        {
            this.text = text;
        }

        protected override void OnInit()
        {
            DrawColor = Color.White;
            //spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>("SubtitlesFont2");
            //spriteFont.Spacing = -3f;
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>("Subtitles1");
        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = PositionAbsolute;
            Vector2 posPixels = Screen.ToPixels(pos );
            Screen.spriteBatch.DrawString(spriteFont, text, posPixels, this.DrawColor);
        }
    }
}
