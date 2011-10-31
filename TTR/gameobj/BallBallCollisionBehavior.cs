// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTR.gameobj
{
    /**
     * hard pseudo-physics collissions like billiard balls, between balls
     */
    class BallBallCollisionBehavior: Gamelet
    {
        Gamelet lastCollidedWith = null;

        protected override void OnInit()
        {
            base.OnInit();

            Parent.OnCollisionEvent += new GameletEventHandler(OnCollision);
        }

        public void OnCollision(Gamelet item, GameletEventArgs e)
        {
            Gamelet withItem = e.otherItem;

            // TODO reset the lastCollide when not-colliding
            if (lastCollidedWith != withItem)
            {
                if (withItem is Ball)
                {
                    Ball ball = withItem as Ball;

                    // swap the velocity vecs - pseudo-phyics eff
                    Vector2 v = ball.Velocity;
                    ball.Velocity = item.Velocity;
                    item.Velocity = v;
                }
                lastCollidedWith = withItem;
            }
        }
    }
}
