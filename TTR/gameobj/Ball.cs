// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTR.main;

namespace TTR.gameobj
{
    /**
     * a ball projectile that goes in straight line towards a target position and bounces back
     */
    public class Ball : Spritelet
    {
        public float Mass = 1f;
        public float CollisionScale = 0.2f;

        public NRGcharge nrgCharge = null;
        public int NrShipCollides = 0;

        public Ball(string shapeFileName)
            : base(shapeFileName)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            ChecksCollisions = true;
            LayerDepth = 0.5f + (float)ID * 0.0001f;
        }

        Vector2 targetPos;
        float speed = 0f, trajectoryDuration = 0f;

        /**
         * will aim with a straight trajectory towards this target
         */
        public Vector2 TargetPosition {
            get
            {
                return targetPos;
            }
            set
            {
                targetPos = value;
            }
        }

        /**
         * first set TargetPosition and Position before setting VelocityScalar. Velocity with which ball will move.
         */
        public float VelocityScalar
        {
            set{                
                Vector2 trajectory = (targetPos - Position) ;
                speed = value;
                trajectoryDuration = trajectory.Length() / speed;
                Velocity = trajectory / trajectory.Length() * speed;
            }
        }

        /**
         * the transit duration associated to this Ball. 
         * This is typically the time it needs to move across the screen to finally 'land' at the player ship
         */
        public float TrajectoryDuration
        {
            get{
                return trajectoryDuration;
            }
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            Vector2 pos = PositionAbsolute;
            // remove if away from screen at the left
            if (pos.X < -0.1f || pos.Y < -0.5f || pos.Y > 1.5f )
            {
                this.Delete = true;
                // FIXME - only remove items after a specified lifetime!
            }
            // remove if away at the right
            else if (pos.X > 2f && NrShipCollides > 0 ) // TODO bound set
            {
                this.Delete = true;
            }
        }

        public override void OnCollision(Spritelet withItem)
        {
            base.OnCollision(withItem);
            if (withItem is Ship)
                NrShipCollides++;
        }

        public override bool CollidesWith(Spritelet item)
        {
            if (item is Ship)
            {
                return (item as Ship).CollidesWith(this);
            }
            if (item is Ball)
            {
                if ((PositionAbsolute - item.PositionAbsolute).Length() < CollisionScale * (RadiusAbsolute + item.RadiusAbsolute))
                    return true;
                else
                    return false;
            }
            return false;
        }

    }
}
