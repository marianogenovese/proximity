using System;
using System.Collections.Generic;
using System.Text;
using testDynamicZoneLib.Models;

namespace testDynamicZoneLib
{
    public class BeaconScanner : EstimoteProximityScanner
    {
        private readonly Sector sector;
        private Guid[] releatedObjects;

        public BeaconScanner(ProximityObserver estimoteProximityObserver, string tag, Sector sector) : base(estimoteProximityObserver, tag, sector.Id, sector.Zone.Distance)
        {
            this.sector = sector;
        }

        protected override Guid[] ReleatedObjects
        {
            get
            {
                if (this.releatedObjects == null)
                {
                    //Calculo la lista de beacons que debo detectar
                    List<Guid> ids = new List<Guid>();
                    foreach (Beacon beacon in this.sector.Beacons)
                    {
                        ids.Add(beacon.Id);
                    }

                    this.releatedObjects = ids.ToArray();
                }

                return this.releatedObjects;
            }
        }

        protected override void OnObjectDetected(ObjectDetected objectDetected)
        {
            if (this.Validate(objectDetected)) //Aqui valida si el beacon detectado (objectId) pertenece a este sector
            {
                //Aqui es importante que no se produzca un objecto detectado con el id del beacon sino con el id del sector
                //para que SectorScanner valide en sus sectores asociados
                //Source.OnNext(new ObjectDetected(this.sector.Id, objectDetected.Type));
                this.PublishEvent(new ObjectDetected(this.sector.Id, objectDetected.Type));
            }
        }
    }
}
