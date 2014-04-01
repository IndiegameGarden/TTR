// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTR.main
{
    public class Player
    {
        public float Score {
            get { return score; }
            set { score=value; }
        }
        public float NRG
        {
            get { return nrg; }
            set { nrg = value; }
        }

        private float score = 0f, nrg = 0f;

        public Player()
        {
        }

    }
}
