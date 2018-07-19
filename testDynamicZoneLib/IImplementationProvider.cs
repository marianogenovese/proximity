using System;
using testDynamicZoneLib.Models;

namespace testDynamicZoneLib
{
    interface IImplementationProvider
    {
        Implementation FindByBeaconId(Guid id);
    }
}
