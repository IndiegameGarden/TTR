// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using TTengine.Core;


namespace TTR.gameobj
{
    public class BallAvoidanceBehavior: Gamelet
    {
        public BallAvoidanceBehavior()
        {
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            Ball me = Parent as Ball;
            if (me.nrgCharge != null && me.nrgCharge.Charged) return;

            Vector2 mePos = this.PositionAbsolute;
            Vector2 meVelocity = me.Velocity;
            float   meRadius = me.RadiusAbsolute;
            Vector2 vChange = Vector2.Zero;
            float meSpeed = meVelocity.Length();
            float distRange = 9.0f *  meRadius ;

            // loop all other balls around
            foreach (Spritelet s in Screen.collisionObjects)
            {
                if (s is Ball && s != me )
                {
                    // skip to next, if other ball is far away 
                    Ball b = s as Ball;
                    Vector2 bPos = b.PositionAbsolute;
                    Vector2 v = bPos - mePos;
                    float dist = v.Length();
                    if (dist > distRange)
                        continue;

                    // skip if other ball is leftish of me
                    if (meVelocity.X > 0f && bPos.X < mePos.X)
                        continue;

                    // calc force of change
                    float f = ( me.Mass/b.Mass ) /(dist*dist + 0.001f) ;
                    Vector2 vNormal ;
                    if (bPos.Y <= mePos.Y)
                    {
                        vNormal = new Vector2(-v.Y,v.X);
                    }else{
                        vNormal = new Vector2(v.Y,-v.X);
                    }
                    vNormal.Normalize();

                    if (bPos.X < mePos.X + meRadius )
                        vChange += 0.0005f * f * vNormal;
                    else
                        vChange += 0.003f * f * vNormal;

                    if (dist < distRange / 2.0f)
                    {
                        // set small change in other ball as well                       
                        Vector2 bv = b.Velocity;
                        float bvl = bv.Length();
                        bv += 0.0002f * f * -vNormal;
                        bv.Normalize();
                        bv *= bvl;
                        b.Velocity = bv;
                    }
                 
                }
            }

            // adjust velocity with the change that resulted from above loop over balls
            meVelocity += vChange;
            // adjust velocity magnitude to what it was
            meVelocity.Normalize();
            Parent.Velocity = meVelocity * meSpeed;

        }

    }
}
