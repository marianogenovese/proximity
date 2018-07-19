using System;

namespace testDynamicZoneLib
{
    public class ImplementationScanner : EstimoteProximityScanner
    {
        public ImplementationScanner(ProximityObserver estimoteProximityObserver, string tag, double distance) : base(estimoteProximityObserver, tag, Guid.Empty, distance)
        {
        }

        protected override Guid[] ReleatedObjects
        {
            get
            {
                return new Guid[] { };
            }
        }

        protected override bool Validate(ObjectDetected objectDetected)
        {
            //Para que cualquier objecto que detecte genere un evento
            return true;
        }
    }
}
