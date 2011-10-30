// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTR.main;
using TTengine;

namespace TTR.gameobj
{
    /**
     * Game of Life used in a shader eff
     */
    public class GoLEffect: Spritelet
    {
        Effect eff;
        SpriteBatch spriteBatch;
        RenderTarget2D renderBufOutput, renderBufInput;
        float dtAfterGolUpdate, golUpdatePeriod;
        bool needFirstUpdate;
        BlendState blendState;
        EffectParameter effDeltaPixelX, effDeltaPixelY, effTime, effDoGolUpdate;

        public GoLEffect(string textureFile)
            : base(textureFile)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            Texture2D texture = Texture;

            dtAfterGolUpdate = 0f;
            golUpdatePeriod = 60f / 140f;
            needFirstUpdate = true;

            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
            if (!RunningGameState.IsXNAHiDef)
                eff = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/GoL_LQ");
            else
                eff = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/GoL");
            effTime = eff.Parameters["Time"];
            effDeltaPixelX = eff.Parameters["DeltaPixelX"];
            effDeltaPixelY = eff.Parameters["DeltaPixelY"];
            effDoGolUpdate = eff.Parameters["DoGolUpdate"];
            effDeltaPixelX.SetValue(1f/((float)texture.Width));
            effDeltaPixelY.SetValue(1f / ((float)texture.Height));
            VertexShaderInit(eff);

            renderBufInput = new RenderTarget2D(spriteBatch.GraphicsDevice, texture.Width, texture.Height);
            renderBufOutput = new RenderTarget2D(spriteBatch.GraphicsDevice, texture.Width, texture.Height);
            blendState = new BlendState();
            blendState.AlphaDestinationBlend = Blend.Zero;
            // first time rendering into buffer using BufferInit technique
            eff.CurrentTechnique = eff.Techniques[0];
            spriteBatch.Begin(SpriteSortMode.Deferred,blendState,null,null,null,eff);
            spriteBatch.GraphicsDevice.SetRenderTarget(renderBufInput);
            spriteBatch.Draw(texture, renderBufInput.Bounds, Color.White);
            spriteBatch.End();
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            dtAfterGolUpdate += p.dt;

            if (needFirstUpdate || dtAfterGolUpdate >= golUpdatePeriod)
            {
                effDoGolUpdate.SetValue(1.0f);
                needFirstUpdate = false;
                dtAfterGolUpdate -= golUpdatePeriod;
            }
            else
            {
                effDoGolUpdate.SetValue(0.0f);
            }
            effTime.SetValue(p.simTime);
            RenderTargetBinding[] rt_old = spriteBatch.GraphicsDevice.GetRenderTargets();
            //spriteBatch.GraphicsDevice.SetRenderTargets(new RenderTargetBinding[] {rtBuf,null} );
            spriteBatch.GraphicsDevice.SetRenderTarget(renderBufOutput);

            // switch to the Update technique
            eff.CurrentTechnique = eff.Techniques[1];
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, eff);
            spriteBatch.Draw(renderBufInput, Vector2.Zero, null, DrawColor);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTargets(rt_old);

            // buffer swap
            RenderTarget2D temp = renderBufInput;
            renderBufInput = renderBufOutput;
            renderBufOutput = temp;
        }
        
        
        protected override void OnDraw(ref DrawParams p)
        {
            // switch to the Draw technique
            eff.CurrentTechnique = eff.Techniques[2];
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, eff);
            spriteBatch.Draw(renderBufInput, Screen.ToPixels(DrawPosition), null, DrawColor,
                   this.RotateAbsolute, DrawCenter, DrawScale, SpriteEffects.None, LayerDepth);
            spriteBatch.End();

        }        
    
    
    }
}
