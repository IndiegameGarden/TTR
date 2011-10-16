// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using TTMusicEngine;
using TTR.gameobj;
using TTR.gfx;

namespace TTR.level
{
    class LevelTrifonicBroken : Level
    {
        const double BPM = 120.0;
        double T = 60.0 / BPM;
        SoundEvent script = new SoundEvent();

        public override SoundEvent CreateSoundScript()
        {
            SampleSoundEvent amb = new SampleSoundEvent("Broken_Packet5/5BrknAmbVamp.wav");
            SampleSoundEvent bass = new SampleSoundEvent("Broken_Packet5/5BrknBassVamp.wav");
            SampleSoundEvent drum = new SampleSoundEvent("Broken_Packet5/5BrknDrumVamp.wav");
            SampleSoundEvent vox = new SampleSoundEvent("Broken_Packet5/5BrknVoxVamp.wav");
            double durLoop = drum.Duration; // single loop duration
            amb.Repeat = 5;
            bass.Repeat = 7;
            drum.Repeat = 8;
            vox.Repeat = 1;
            script.AddEvent(5 /*durLoop*/, amb);
            script.AddEvent(0, bass);
            script.AddEvent(0, drum);
            //script.AddEvent(durLoop, vox);

            // add game event to 'amb'
            BallShape ballShape = new BallShape(0);
            Ball ballItem = new Ball(ballShape, new Vector2(1500f,300f),new Vector2(0f,300f), 400.0f);
            ItemCreateSoundEvent ice = new ItemCreateSoundEvent(ballItem);
            ice.AttachTo(amb);
            return script;
        }

        public override double GetBPM()
        {
            return BPM;
        }
    }
}
