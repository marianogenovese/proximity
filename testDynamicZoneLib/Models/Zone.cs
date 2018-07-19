using System;
using System.Collections.Generic;
using System.Text;

namespace testDynamicZoneLib.Models
{
    public class Zone
    {
        private readonly double distance;

        public Zone(double distance)
        {
            this.distance = distance;
        }

        public double Distance
        {
            get
            {
                return this.distance;
            }
        }
    }
}
