// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
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
     * Displays text effects screen for a specified time. Use StartTime and Duration properties to modify if needed
     * supports typical attributes like DrawColor
     */
    public class MegaText : Spritelet
    {
        protected string text;
        protected SpriteFont spriteFont;
        protected float spacing = 0f;

        /// Create an on-screen text item with specified text, using font content fontFile
        public MegaText( string text, string fontFile)
            : base()
        {
            this.text = text;
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>(fontFile);
            Spacing = -10f;
            LayerDepth = 0.9f;
        }

        /// Font spacing
        public float Spacing
        {
            get
            {
                return spacing;
            }
            set
            {
                spacing = value;
                spriteFont.Spacing = spacing;
            }
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            Spacing += p.dt * 5f;
        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 posPixels = Screen.ToPixels(PositionAbsolute );
            Screen.spriteBatch.DrawString(spriteFont, text, posPixels, this.DrawColor, RotateAbsolute, 
                    Vector2.Zero, ScaleAbsolute, SpriteEffects.None, LayerDepth);
        }
    }
}
