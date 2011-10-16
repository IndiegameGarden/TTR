// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using TTR.main;
using TTengine;

namespace TTR.gameobj
{
    /**
     * an energy charge coupled to an item, which triggers a bonus eff (eg score up, music) when
     * hit by the player Ship.
     */
    public class NRGcharge : Spritelet
    {
        public bool Charged = true;
        float pulseMagn = 0.25f + RandomMath.RandomBetween(-0.25f,0.25f);

        public NRGcharge( Color color)
            : base("orb-col_128")
            //: base("galaxy_256_test")
            //: base("Galaxy_NGC_7742")
        {            
            DrawColor = color;
            Alpha = 0.2f;
            Scale = 0.7f;
        }

        // end of life mode set, item will fade away and can not trigger the bonus effects
        protected bool isEndOfLifeMode = false;

        protected override void OnInit()
        {
            base.OnInit();

            Parent.OnCollisionEvent += new GameletEventHandler(OnCollision);
            LayerDepth = 0.8f + (float)ID * 0.0001f;
        }

        // configure NRCcharge by Adding to a Ball
        protected override void OnNewParent()
        {
            base.OnNewParent();

            if ((Parent is Ball) && ((Parent as Ball).nrgCharge == null))
            {
                (Parent as Ball).nrgCharge = this;
                this.Charged = true;
            }
        }

        // check ball2ball and ball2ship collisions
        public void OnCollision(Gamelet sender, GameletEventArgs e)
        {            
            /*
            if (e.otherItem is Ball && Parent is Ball)
            {
                Ball parentBall = Parent as Ball;
                Ball collidingBall =  e.otherItem as Ball;
                if (parentBall.nrgCharge != null)
                {
                    isEndOfLifeMode = true;
                    Charged = false;
                }
            }
             */

            if (e.otherItem is Ship && !isEndOfLifeMode )
            {
                Ship ship = e.otherItem as Ship;
                // uncouple from my parent (whatever it was)
                LinkedToParent = false;
                Position = Parent.PositionAbsolute;
                // reduce velocity
                Velocity = new Vector2(Parent.Velocity.X * 0.75f, Parent.Velocity.Y * 0.75f);
                isEndOfLifeMode = true;
                Charged = false;

                // score
                ship.player.Score += 100.0f;

                // energy increaes
                ship.player.NRG += 2.0f; // TODO
            }

        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            // at end of life, blow up myself and fade away
            if (isEndOfLifeMode)
            {
                Scale += p.dt * 4.6f; //2.4
                DrawColor *= (0.97f); // NOTE assumes a p.dt = 10 ms
                Rotate -= (1.0f * p.dt);
                if (Scale > 4.6f)
                    Delete = true;
            }
            else
            {
                // random flickering scale
                /*
                //float c = 1.2f * p.dt;
                float c = 4f * p.dt;
                Scale += RandomMath.RandomBetween(-c, c);
                if (Scale > 1.5f)
                    Scale = 1.5f - RandomMath.RandomBetween(0, c);
                if (Scale < 0.3f)
                    Scale = 0.3f + RandomMath.RandomBetween(0, c);
                */
                // pulsating with random magnitude
                float c = 0.1f * p.dt;
                pulseMagn += RandomMath.RandomBetween(-c, c);
                ScaleModifier = 1.0f + pulseMagn * (float) Math.Sin( (float) MathHelper.TwoPi * 0.3f * SimTime);
            }
            Rotate += (1.0f * p.dt);
            Rotate = Rotate % (MathHelper.TwoPi); // clip to 0-2pi TODO do this natively??
        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);
        }
    }
}
