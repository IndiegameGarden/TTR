// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine;

namespace TTR.gameobj
{
    /**
     * default ball-to-ship collisions. The specific 'rounded bar' shape of ship is used to calculate ball reaction
     */
    class BallShipCollisionBehavior: Gamelet
    {
        protected override void OnInit()
        {
            base.OnInit();
            // use eventing to add me as listener
            Parent.OnCollisionEvent += new GameletEventHandler(OnCollision);
        }

        public void OnCollision(Gamelet item, GameletEventArgs e)
        {
            if (!(e.otherItem is Ship))
                return;

            Ship ship = e.otherItem as Ship;
            Ball ball = Parent as Ball;
            Vector2 ballPos = ball.PositionAbsolute;
            Vector2 shipPos = ship.PositionAbsolute;
            Vector2 localNormal;
            float h2, r;

            // never let ball bounce back twice in the same period of time.
            if (ball.Velocity.X > 0)
                return;

            if (ballPos.X < (shipPos.X)) // if already left-of ship
                return;

            r = ship.WidthAbsolute / 2;
            h2 = ship.HeightAbsolute / 2 - r;

            // check where ball is: up, down or middle zone of ship
            if (ballPos.Y < shipPos.Y - h2)
            { // up zone
                Vector2 p = new Vector2(shipPos.X, shipPos.Y - h2);
                localNormal = ballPos - p;
                localNormal.Normalize();
            }
            else if (ballPos.Y > shipPos.Y + h2)
            { // down zone
                Vector2 p = new Vector2(shipPos.X, shipPos.Y + h2);
                localNormal = ballPos - p;
                localNormal.Normalize();
            }
            else
            { // middle zone                
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                return;
            }

            // reduce mass after refl
            ball.Mass /= 10.0f;

            //Vector2 newItemVelocity;
            //Vector2 itemVelocity = item.Velocity;            
            //Vector2.Reflect(ref itemVelocity, ref localNormal, out newItemVelocity);
            ball.Velocity = localNormal * ball.Velocity.Length(); //new Vector2(-newItemVelocity.X, newItemVelocity.Y);

        }
    }
}
