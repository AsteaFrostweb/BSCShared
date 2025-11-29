using BSCShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSCShared
{
    public static class Tick
    {

        public static int currentTick;
        public const int tickRate = 10;
        public static float tickDuration => 1.0f / tickRate;
        public const int interpolationDelayTicks = 2;    

    }
}
