//
// La responsabilidad de esta clase es usar el EstimoteScanner para detectar beacons via BT
// los beacons que detecte por BT (OnObjectDetected) deben ser validados para controlar que
// el beacon detectado pertenezca a sus objetos relacionados (ReleatedObjects), si es verdadero
// BeaconScanner va a emitir un evento con el Id del sector no con el Id del beacon puesto que
// un grupo de beacons deben verse como uno solo
//
public class BeaconScanner : EstimoteProximityScanner
{
    private readonly Sector sector;
    private readonly Guid[] releatedObjects;

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
                foreach(Beacon beacon in this.sector.Beacons)
                {
                    ids.Add(beaconId);
                }
                
                this.releatedObjects = ids.ToArray();
            }

            return this.releatedObjects;
        }
    }

    protected override  void OnObjectDetected(EventType type, Guid objectId)
    {
        if(Validate(type, objectId)) //Aqui valida si el beacon detectado (objectId) pertenece a este sector
        {
            //Aqui es importante que no se produzca un objecto detectado con el id del beacon sino con el id del sector
            //para que SectorScanner valide en sus sectores asociados
            Source.onNext(new ObjectDetected(this.sector.Id, type));
        }
    }
}