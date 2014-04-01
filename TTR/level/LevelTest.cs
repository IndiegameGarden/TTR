// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTMusicEngine;
using TTMusicEngine.Soundevents;
using TTengine.Core;
using TTR.gameobj;
using TTR.main;
using TTR.soundevents;

namespace TTR.level
{
    public class LevelTest : Level
    {
        const double BPM = 140.0;
        const double T = 4 * 60.0 / BPM;

        SampleSoundEvent wTrk, wVox, wLead, wBeat, wHat, wStr, wBeep;
        SoundTrackEvent tTrk, tVox, tLead, tBeat, tHat, tStr, tBeep;

        public LevelTest()
            : base("TestLevel")
        {
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
        }

        public override void CreateShip()
        {
            base.CreateShip();
            ship.AddFront(new ShipBallLashbackOnCollisionBehavior());
        }

        public override void CreateContent()
        {

            if (RunningGameState.IsXNAHiDef)
            {
                this.Add(new StarfieldEffect(0.4f));
                this.Add(new PowerRingsSingleton());

                VortexEffect ve = new VortexEffect("CurvedVortex", "clouds");
                //this.Add(ve);
                ve.Position = new Vector2(0.2f, 0.5f);
                ve.VortexVelocity = 0.04f;
                ve.NoiseLevel = 0.03f;
                ve.Active = true;
                ve.Duration = 10f;

                BoomDepthEffect b = new BoomDepthEffect();
                b.StartTime = 3f;
                b.Ampl = 0.2f;
                //this.Add(b);

                MandelBackground mb = new MandelBackground();
                //this.Add(mb);

                this.Add(new StarCloudsEffect());

            }

            // test txt
            SubtitleText st = new SubtitleText("     Captain, MAYDAY!\nWe're running out of NRG!");
            st.Position = new Vector2(0.3f, 0.5f);
            st.Duration = 12f;
            st.StartTime = 2f;
            //this.Add(st);

            MegaText mt = new MegaText("EXPERIMENT", "RkaFont");
            mt.Position = new Vector2(0.2f, 0.4f);
            mt.Duration = 12f;
            mt.StartTime = 4f;
            this.Add(mt);

            ScreenShake shake = new ScreenShake(0.005f, 0.005f, 3.3f);
            shake.StartTime = 5f;
            //this.Add(shake);

            
            PlanetTexture p = new PlanetTexture();
            p.Position = new Vector2(0.65f, 0.5f);
            p.LayerDepth = 0.0f;
            Add(p);
             

        }

        public override void CreateMusicScript()
        {
            musicScript = new SoundTrackEvent(BPM);
            wTrk = new SampleSoundEvent("Sublime/drumnbass.ogg");
            wVox = new SampleSoundEvent("Sublime/vox.ogg");
            wLead = new SampleSoundEvent("Sublime/leads.ogg");
            wBeat = new SampleSoundEvent("Sublime/beat.ogg");
            wHat = new SampleSoundEvent("Sublime/revhat.ogg");
            wStr = new SampleSoundEvent("Sublime/strings.ogg");
            wBeep = new SampleSoundEvent("Sublime/beeps.ogg");


            musicScript.AddEvent(0, wTrk);
            musicScript.AddEvent(0, wVox);
            musicScript.AddEvent(0, wLead);
            musicScript.AddEvent(0, wBeat);
            musicScript.AddEvent(0, wHat);
            musicScript.AddEvent(0, wStr);
            musicScript.AddEvent(0, wBeep);


            double N = Math.Sqrt(musicScript.Children.Count);
            wTrk.Amplitude = 1 / N;
            wVox.Amplitude = 1 / N;
            wLead.Amplitude = 1 / N;
            wBeat.Amplitude = 1 / N;
            wHat.Amplitude = 1 / N;
            wStr.Amplitude = 1 / N;
            wBeep.Amplitude = 1 / N;

            // attach a events soundtrack to each sampled wave track
            tLead = new SoundTrackEvent(BPM);
            wLead.AddEvent(0, tLead);
            tVox = new SoundTrackEvent(BPM);
            wVox.AddEvent(0, tVox);
            tTrk = new SoundTrackEvent(BPM);
            wTrk.AddEvent(0, tTrk);
            tBeat = new SoundTrackEvent(BPM);
            wTrk.AddEvent(0, tBeat);
            tHat = new SoundTrackEvent(BPM);
            wTrk.AddEvent(0, tHat);
            tStr = new SoundTrackEvent(BPM);
            wTrk.AddEvent(0, tStr);
            tBeep = new SoundTrackEvent(BPM);
            wBeep.AddEvent(0, tBeep);

            // volume-decrease events
            // vol( measure.beat-nr,  which-track, measures-duration, game-item )
            for (double t = 3; t < 200; t++)
            {
                Ball b = crBall(0.3f + (float)t/200.0f, 0.4f);
                //b.Mass = 10.0f;
                vol(t + 0.1, tBeat, 1.0, b);
                b = crBall(1.0f - (float)t / 200.0f, 0.45f);
                vol(t + 0.2, tBeat, 1.0, b);

                if (t == 12)
                {
                    /*
                    // FIXME component does not get inited now?
                    VortexEffect ve = new VortexEffect("CurvedVortex", "clouds");
                    b.Add(ve);
                    //ve.Position = new Vector2(2.1f, 0.43f);
                    ve.VortexVelocity = 0.04f;
                    ve.NoiseLevel = 0.03f;
                    ve.Paused = false;
                    ve.Duration = 10f;
                     */
                }
            }
            /*
            vol(5.1, tBeat, 4.0, crBall(0.5f, 1.26f));
            vol(7.2, tBeat, 4.0, crBall(0.7f, 1.18f));
            vol(9.2, tBeat, 4.0, crBall(0.7f, 1.18f));
            vol(11.2, tBeat, 4.0, crBall(0.7f, 1.18f));
            vol(13.2, tBeat, 4.0, crBall(0.7f, 1.18f));
             */

        }

        Ball crBall(float y, float spd)
        {
            Ball b = new Ball("ball-supernova"); //, new Vector2(2.1f, y), new Vector2(collideXposDefault, y), spd);
            b.Position = new Vector2(2.1f, y);
            float collideXposDefault = ship.Position.X + ship.WidthAbsolute / 2 + b.RadiusAbsolute;
            b.TargetPosition = new Vector2(collideXposDefault, y);
            b.VelocityScalar = spd;

            PowerRingWhenShipHitBehavior eff = new PowerRingWhenShipHitBehavior();
            eff.RingWidth = 5.0f; // TODO higher is less wide
            eff.RingRadius = 0.07f;
            //eff.RingColor = Color.DarkGreen;
            eff.RingColor = Color.Tomato;
            eff.Alpha = 0.5f;
            b.Add(eff);

            b.Add(new NRGcharge(Color.White));

            b.Add(new BallAvoidanceBehavior());

            //b.Add(new BallBallCollisionBehavior());

            b.Add(new BallShipCollisionBehavior());

            return b;
        }

        void vol(double beatTime, SoundTrackEvent track, double measuresDuration, Ball item)
        {
            this.Add(item);

            if (item.nrgCharge != null)
            {
                SoundEvent sev = new VolDimSoundEvent(item.nrgCharge, measuresDuration * (4 * 60 / BPM));
                track.b(beatTime, sev); // TODO rename method b ?
                item.StartTime = (float)Beat.BeatToTime(beatTime, BPM) - item.TrajectoryDuration;
            }
        }

    }
}
