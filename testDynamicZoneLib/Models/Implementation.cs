using System;
using System.Collections.Generic;
using System.Text;

namespace testDynamicZoneLib.Models
{
    public class Implementation
    {
        private readonly Guid id;
        private readonly Sector[] sectors;

        public Implementation(Guid id, Sector[] sectors)
        {
            this.id = id;
            this.sectors = sectors;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public Sector[] Sectors
        {
            get
            {
                return this.sectors;
            }
        }
    }
}
