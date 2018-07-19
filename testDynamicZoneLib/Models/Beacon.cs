using System;
using System.Collections.Generic;
using System.Text;

namespace testDynamicZoneLib.Models
{
    public class Beacon
    {
        private readonly Guid id;

        public Beacon(Guid id)
        {
            this.id = id;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
