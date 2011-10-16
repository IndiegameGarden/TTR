// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using TTR;
using TTR.gameobj;
using TTengine;

namespace TTR.main
{
    public class ShipShape : Spritelet /* LODSpritelet */
    {

        public ShipShape()
            : base("ship_28")
            //: base(new string[] { "ship_28", "ship_150", "ship_342"}) //, "ship_1200_mid" }) // "
            //: base(new string[]{"ship_28","ship_147","ship_640"}) // "
            //: base(new string[]{"ship_28","ship_147"}) // "
            //: base(new string[]{"ship_28"} ) 
            //: base("ship_342") 
        {
            this.LayerDepth = 0.1f; // FIXME all layerdepths in a defines file
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            // 0.08187134503f *   // if only _342 used
            //Scale = (0.19f + 20.0f * (1f + (float)Math.Sin(MathHelper.TwoPi * 0.1f * SimTime)) );
        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            // text
            Vector2 pos = PositionAbsolute;
            String s = "<" + Math.Round(pos.X,3) + "," + Math.Round(pos.Y,3) + ">";
            Screen.DebugText(0.4f, 0.95f, s);
        }

    }
}
