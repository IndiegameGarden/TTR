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
    public class TimewarpLogo: Spritelet
    {
        Effect eff;
        SpriteBatch spriteBatch;
        EffectParameter effTime;

        public TimewarpLogo(String fileName): base(fileName)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            //Texture2D texture = Texture;

            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
            eff = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/TimewarpLogo");
            effTime = eff.Parameters["Time"];
            VertexShaderInit(eff);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            effTime.SetValue(p.simTime);
        }


        protected override void OnDraw(ref DrawParams p)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, eff);
            spriteBatch.Draw(Texture, Screen.ToPixels(DrawPosition), null, DrawColor,
                   RotateAbsolute, DrawCenter, DrawScale, SpriteEffects.None, LayerDepth);
            spriteBatch.End();

        }        

    }
}
