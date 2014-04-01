// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTR.gameobj
{
    /** attach to a Ship to get 'lashback' when a Ball hits it
     */
    class ShipBallLashbackOnCollisionBehavior: Gamelet
    {
        protected override void OnInit()
        {
            base.OnInit();
            // use eventing to add me as listener
            Parent.OnCollisionEvent += new GameletEventHandler(OnCollision);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
        }

        public void OnCollision(Gamelet item, GameletEventArgs e)
        {
            if (!(e.otherItem is Ball))
                return;

            Ship ship = Parent as Ship;
            Ball ball = e.otherItem as Ball;

            // impact vector of Ball
            Vector2 v = ball.Velocity;
            v.Y = -v.Y;

            // calc rotation
            float dy = (ball.PositionAbsolute.Y - ship.PositionAbsolute.Y);
            float rotAmpl = ball.Mass * dy / 1.5f;
            ship.AddFront(new LashbackBehavior(ball.Mass * v / 30.0f, rotAmpl));
            //ball.Mass /= 10.0f;
        }

    }

    class LashbackBehavior: Gamelet
    {
        Vector2 vImpact;
        float rotAmpl;

        public LashbackBehavior(Vector2 vImpact, float rotAmpl): base()
        {
            this.vImpact = vImpact;
            this.rotAmpl = rotAmpl;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            float f = 2.0f;
            if (this.SimTime > 0.5f / f)
            {
                Delete = true; // TODO, may add item to a "to be deleted" list when Delete is called! requires to block direct access to 'bool delete'.
            }
            else
            {
                //Parent.PositionModifier += new Vector2(0.2f, 0f);
                if(vImpact.X >= 0)
                    Parent.PositionModifier += new Vector2(-vImpact.X, vImpact.Y) * (float)Math.Sin(MathHelper.TwoPi * f * this.SimTime);
                else
                    Parent.PositionModifier += vImpact * (float)Math.Sin(MathHelper.TwoPi * f * this.SimTime);
                Parent.RotateModifier += rotAmpl * (float)Math.Sin(MathHelper.TwoPi * f * this.SimTime);
            }
        }
    }

}
