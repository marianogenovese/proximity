using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using testDynamicZoneLib.Models;

namespace testDynamicZoneLib
{
    public class SectorScanner : ProximityScanner<ObjectDetected>
    {
        private Guid[] releatedObjects;
        private Subject<ObjectDetected> customObservable;
        private readonly Implementation implementation;
        private readonly ProximityObserver estimoteProximityObserver;
        private IObservable<ObjectDetected> beaconsInSectorDetectedObservable;
        private IDisposable sectorObserver;

        public SectorScanner(ProximityObserver estimoteProximityObserver, Implementation implementation) : base(implementation.Id, 0)
        {
            this.implementation = implementation;
            this.estimoteProximityObserver = estimoteProximityObserver;
        }

        protected override Guid[] ReleatedObjects
        {
            get
            {
                if (this.releatedObjects == null)
                {
                    //Calculo la lista de sectores que debo detectar
                    List<Guid> ids = new List<Guid>();
                    foreach (Sector sector in implementation.Sectors)
                    {
                        ids.Add(sector.Id);
                    }

                    this.releatedObjects = ids.ToArray();
                }

                return this.releatedObjects;
            }
        }

        protected override IObservable<ObjectDetected> Source
        {
            get
            {
                if (customObservable == null)
                {
                    customObservable = new Subject<ObjectDetected>();
                }

                return customObservable;
            }
        }

        // Sobreescribo el metodo porque solo la primera vez que un observador se suscriba,
        // debo crear la BeaconScanner(s) asociados al sector y unirlo de ser el caso para observar
        // varios sectores de una sola vez.
        public override IDisposable Subscribe(IObserver<ObjectDetected> observer)
        {
            if (beaconsInSectorDetectedObservable == null)
            {
                List<BeaconScanner> scanners = new List<BeaconScanner>();
                foreach (Sector sector in this.implementation.Sectors)
                {
                    if (beaconsInSectorDetectedObservable == null)
                    {
                        beaconsInSectorDetectedObservable = new BeaconScanner(this.estimoteProximityObserver, "CustomAuparAzone", sector);
                    }
                    else
                    {
                        beaconsInSectorDetectedObservable = beaconsInSectorDetectedObservable.Merge(new BeaconScanner(this.estimoteProximityObserver, "CustomAuparAzone", sector));
                    }
                }

                this.sectorObserver = beaconsInSectorDetectedObservable.Subscribe((objectDetected) =>
                {
                    this.OnObjectDetected(objectDetected);
                });
            }

            return Source.Subscribe(observer);
        }

        public override void Dispose()
        {
            //if (beaconsInSectorDetectedObservable != null)
            //{
            //beaconsInSectorDetectedObservable.Dispose();
            //}

            if (this.customObservable != null)
            {
                this.customObservable.Dispose();
            }

            if (this.sectorObserver != null)
            {
                this.sectorObserver.Dispose();
            }
        }

        protected override void PublishEvent(ObjectDetected objectDetected)
        {
            this.customObservable.OnNext(objectDetected);
        }
    }
}
