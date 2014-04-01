// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿
/**
 * file contains all classes related to Power Rings, which are objects rendered
 * via a shader eff. The shader is in a single PowerRingsSingleton class
 * which must be added to the game tree.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TTengine.Core;
using TTR.main;

namespace TTR.gameobj
{
    /**
     * a behavior that can be attached to a ball, to provide a circle of light eff at the moment of bounce/collision.
     */
    public class PowerRingWhenShipHitBehavior : PowerRingEffect
    {
        bool hasFired = false;
        float growthTime = 0f;

        public PowerRingWhenShipHitBehavior()
            : base()
        {
            LinkedToParent = false;
            Visible = false;
            //Alpha = 0.5f;
        }

        public override void OnCollision(Spritelet gi)
        {
            base.OnCollision(gi);

            if (gi is Ship)
            {
                Ball b = Parent as Ball;
                Position = b.Position; // new Vector2(0.5f, 0.5f); // b.Position;
                hasFired = true;
                Visible = true;
            }
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            if (hasFired)
            {
                growthTime += p.dt;
                // increase ring size
                if (RingRadius < 3f)
                    RingRadius = 0.001f + growthTime * 2f;
                else
                    Visible = false; // shut off after not visible anymore
                if (RingWidth > 0.5f)
                    RingWidth  -= p.dt * 5f;

                // fade away as parent moves further away?
            }
        }
    }

    /**
     * a single power ring that can be used general-purpose. It has properties like
     * radius and ring width that can be set. Note only limited nr of rings at the same
     * time can be supported by the shader through slots (see MAX_SLOTS).
     * Slots are assigned in round-robin fashion.
     */
    public class PowerRingEffect : Spritelet
    {
        // TODO color
        // TODO some random disturbance of ring radius? see 'noise' hlsl
        public float RingRadius { get; set; }
        public float RingWidth {get; set; }
        public Color RingColor { get { return new Color(ringColorVector4); } set { ringColorVector4 = value.ToVector4(); } }
        public const int MAX_SLOTS = 4;

        private Vector4 ringColorVector4;
        private int slotIndex;
        
        public PowerRingEffect()
            : base()
        {
            slotIndex = -1; // means no slot claimed yet
        }

        protected override void OnDelete()
        {
            if (slotIndex >= 0)
                PowerRingsSingleton.Instance.aSizes[slotIndex] = 0f; // says to fx to not draw this slot
            slotIndex = -1;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            // if became invisible again, release my slot.
            if (!Visible && slotIndex>=0)
            {
                PowerRingsSingleton.Instance.aSizes[slotIndex] = 0f;
                slotIndex = -1;
            }

            if (Visible)
            {
                // if have to (re)claim my slot
                if (slotIndex == -1)
                {
                    // find a free slot
                    for (int i = 0; i < MAX_SLOTS; i++)
                    {
                        if (PowerRingsSingleton.Instance.aSizes[i] == 0f) // claim it
                        {
                            slotIndex = i;
                            break;
                        }
                    }
                }
                // only adapt slot vars if I found a free slot.
                if (slotIndex >= 0)
                {
                    // set the vars to most current ones
                    PowerRingsSingleton.Instance.aPositions[slotIndex] = new Vector2(PositionAbsolute.X / Screen.AspectRatio, PositionAbsolute.Y); // convert to (0-1,0-1) fx coords
                    PowerRingsSingleton.Instance.aSizes[slotIndex] = RingRadius;
                    PowerRingsSingleton.Instance.aWidths[slotIndex] = RingWidth;
                    ringColorVector4.W = Alpha;
                    PowerRingsSingleton.Instance.RingColor = ringColorVector4;
                }
            }
        }

    }

    /**
     * singleton that draws multiple PowerRing effects with one shader
     * 
     */
    public class PowerRingsSingleton : Gamelet
    {
        private const int MAX_SLOTS = 8;
        // fx related
        private Effect eff;
        private EffectParameter effectRingColor;
        private EffectParameter effectPositions;
        private EffectParameter effectSizes;
        private EffectParameter effectAspectRatio;
        private EffectParameter effectWidths;
        private Texture2D texture;
        SpriteBatch spriteBatch;
        private static PowerRingsSingleton instance = null;

        internal Vector4 RingColor = Color.Red.ToVector4();
        internal Vector2[] aPositions = new Vector2[MAX_SLOTS];
        internal float[] aSizes = new float[MAX_SLOTS];
        internal float[] aWidths = new float[MAX_SLOTS];

        /// Warning - only single instance allowed!
        public PowerRingsSingleton()
            : base()
        {
            if (instance != null) throw new Exception("This should be a singleton, one instance created only");
            instance = this;
        }

        public static PowerRingsSingleton Instance
        {
            get
            {
                return instance;
            }
        }
        
        protected override void OnInit()
        {
            texture = TTengineMaster.ActiveGame.Content.Load<Texture2D>("clouds");
            eff = TTengineMaster.ActiveGame.Content.Load<Effect>("Effects/PowerRings");
            effectRingColor = eff.Parameters["RingColor"];
            effectPositions = eff.Parameters["aPosition"];
            effectSizes = eff.Parameters["aSize"];
            effectWidths = eff.Parameters["aWidth"];
            effectAspectRatio = eff.Parameters["AspectRatio"];
            effectAspectRatio.SetValue(Screen.AspectRatio);
            spriteBatch = new SpriteBatch(Screen.graphicsDevice);

            VertexShaderInit(eff);
        }

        protected override void OnDraw(ref DrawParams p)
        {
            Rectangle drawRect = Screen.ScreenRectangle;

            effectRingColor.SetValue(RingColor);
            effectPositions.SetValue(aPositions); 
            effectSizes.SetValue(aSizes);
            effectWidths.SetValue(aWidths);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, eff);
            spriteBatch.Draw(texture, drawRect, null, Color.White, 0.0f,
                Vector2.Zero, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }


    }

}
