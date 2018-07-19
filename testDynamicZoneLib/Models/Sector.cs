using System;
using System.Collections.Generic;
using System.Text;

namespace testDynamicZoneLib.Models
{
    public class Sector
    {
        private readonly Guid id;
        private readonly Zone zone;
        private readonly Beacon[] beacons;

        public Sector(Guid id, Zone zone, Beacon[] beacons)
        {
            this.id = id;
            this.zone = zone;
            this.beacons = beacons;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public Zone Zone
        {
            get
            {
                return this.zone;
            }
        }

        public Beacon[] Beacons
        {
            get
            {
                return this.beacons;
            }
        }
    }
}
