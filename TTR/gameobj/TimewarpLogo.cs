using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTR.main;
using TTengine;
using TTengine.Core;

namespace TTR.gameobj
{
    public class TimewarpLogo: EffectSpritelet
    {    
        EffectParameter effTime;

        public TimewarpLogo(String fileName): base(fileName,"Effects/TimewarpLogo")
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            //Texture2D texture = Texture;
            effTime = eff.Parameters["Time"];
            spriteSortMode = SpriteSortMode.Deferred;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            effTime.SetValue(p.simTime);
        }

    }
}
