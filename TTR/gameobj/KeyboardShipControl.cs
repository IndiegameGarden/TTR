// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using TTR.main;
using TTengine;

namespace TTR.gameobj
{
    public class KeyboardShipControl : Gamelet
    {
        const float SHIP_ACC_MAX = 84.0f;
        const float SHIP_VEL_MAX = 1.0f;
        const float SHIP_DEFAULT_DECEL = 18f; 

        Ship parentShip = null;

        public KeyboardShipControl()
            : base()
        {
        }

        protected override void OnNewParent()
        {
            parentShip = Parent as Ship;
            parentShip.velocityMax = SHIP_VEL_MAX;
            parentShip.defaultDeceleration = SHIP_DEFAULT_DECEL;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            float accy = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                accy = -SHIP_ACC_MAX;
                //parentShip.Velocity = new Vector2(0f,-SHIP_VEL_MAX);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                accy = +SHIP_ACC_MAX;
                //parentShip.Velocity = new Vector2(0f, +SHIP_VEL_MAX);
            }
            else
            {
                // 0
                //parentShip.Velocity = Vector2.Zero;
            }

            // acc brake
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                parentShip.velocityMax = SHIP_VEL_MAX / 3.0f;
            else
                parentShip.velocityMax = SHIP_VEL_MAX ;

            parentShip.driveAcceleration = new Vector2(0, accy);

        }

    }
}
