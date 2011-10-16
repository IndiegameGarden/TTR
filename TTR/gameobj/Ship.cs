// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine;
using TTR.main;

namespace TTR.gameobj
{
    public class Ship : Spritelet
    {
        // speed in pixel/s , acc in pixel/s^2
        public float velocityMax = float.PositiveInfinity;
        public float defaultDeceleration = 0f;
        public float NrgBurnRate = 1.0f;
        public Vector2 driveAcceleration = Vector2.Zero;
        public Player player;
        public ShipShape shipShape;

        float minY = 0f;
        float maxY = 1f;

        public Ship()
            : base()
        {            
            shipShape = new ShipShape();
            Add(shipShape);

        }

        protected override void OnInit()
        {
            base.OnInit();

            ChecksCollisions = true;
            Position = new Vector2(0.05f, maxY / 2);
        }

        protected void EnforcePosLimits()
        {
            // cap on position
            if (Position.Y < minY)
            {
                Position.Y = minY;
                Acceleration.Y = 0f;
                Velocity.Y = 0f;
            }
            if (Position.Y> maxY)
            {
                Position.Y = maxY;
                if (Acceleration.Y > 0)
                    Acceleration.Y = 0f;
                if (Velocity.Y > 0)
                    Velocity.Y = 0f;
            }

            // cap on max velocity
            if (Velocity.Y > velocityMax)  Velocity.Y = velocityMax;
            if (Velocity.Y < -velocityMax) Velocity.Y = -velocityMax;
            if (Velocity.X > velocityMax)  Velocity.X = velocityMax;
            if (Velocity.X < -velocityMax) Velocity.X = -velocityMax;            

        }

        
        protected override void OnUpdate(ref UpdateParams p)
        {
            height = shipShape.HeightAbsolute;
            width = shipShape.WidthAbsolute;
            maxY = Screen.Height;

            Acceleration = Vector2.Zero;
            if (driveAcceleration.LengthSquared() > 0)
            {
                Acceleration = driveAcceleration;
            }

            //-- decelerate by default (after applying acc/vel above)
            Vector2 accBreak = Vector2.Negate(Velocity);
            if (driveAcceleration.LengthSquared() == 0f && accBreak.LengthSquared() > 0f)
            {
                accBreak.Normalize();  // determine (unity) direction opposite to where we go now.
                accBreak *= defaultDeceleration ;
                // check if we can stop right now with this deceleration
                if (accBreak.LengthSquared()/2 >= Velocity.LengthSquared())
                {
                    Velocity = Vector2.Zero;
                    Acceleration = Vector2.Zero;
                }
                else
                {
                    Acceleration = accBreak;
                }
            }

            EnforcePosLimits();

            // energy use of ship
            player.NRG -= NrgBurnRate * p.dt;
            if (player.NRG < 0)
                player.NRG = 0f;
        }

        public override bool CollidesWith(Spritelet item)
        {
            if (item is Ball)
            {
                float itemR = item.RadiusAbsolute;
                Vector2 itemPos = item.PositionAbsolute;
                Vector2 pos = PositionAbsolute;
                float ytop = pos.Y - HeightAbsolute / 2;
                float ybot = pos.Y + HeightAbsolute / 2;
                float r = 1.0f * RadiusAbsolute ; // radius of tips of the ship with minor corr factor

                // step 1) first weed out the clear 'not collide' cases (see log 3/3/11)
                if (itemPos.Y + itemR < ytop)  // if above ship
                    return false;
                if (itemPos.Y - itemR > ybot)  // if below ship
                    return false;
                if (itemPos.X - itemR > (pos.X + r)) // if right-of ship
                    return false;
                if (itemPos.X + itemR < (pos.X - r)) // if already left-of ship
                    return false;

                // step 1b) case where ball is too leftish of ship
                if (itemPos.X < (pos.X - r)) // if already left-of ship
                    return false;

                // step 2) check if 'in' pTop/pBot defined points
                Vector2 pTop = new Vector2(pos.X, ytop + r);
                Vector2 pBot = new Vector2(pos.X, ybot - r);
                if ((pTop - itemPos).Length() <= (itemR + r))
                    return true;
                if ((pBot - itemPos).Length() <= (itemR + r))
                    return true;

                // step 3) check if in body
                if (itemPos.Y > ytop && itemPos.Y < ybot)
                    return true;
            }

            // all other cases, default
            return false;
        }

        public override void OnCollision(Spritelet item)
        {
            base.OnCollision(item);
        }

    }
}
