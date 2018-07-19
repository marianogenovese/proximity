//
// Esta clase es responsable de detectar sectores dentro de una implementación.
// por lo que usa la clase BeaconScanner para detectar un grupo de beacons asociados a un sector.
// Finalmente, si se deben escanear multiples sectores se hacen un solo observable via operador Merge,
// que permite unir observables de BeaconScanner.
//
public class SectorScanner :  ProximityScanner<ObjectDetected>
{
    private readonly Guid[] releatedObjects;
    private readonly IObservable<ObjectDetected> customObservable;
    private readonly Implementation implementation;
    private readonly ProximityObserver estimoteProximityObserver;
    private IObservable<ObjectDetected> beaconsInSectorDetectedObservable;

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
                foreach(Sector sector in implementation.Sectors)
                {
                    ids.Add(sector.Id);
                }
                this.releatedObjects = ids.ToArray();
            }

            return this.releatedObjects;
        }
    }

    protected override IObservable<T> Source
    {
        get
        {
            if (customObservable == null)
            {
                customObservable = new CustomObservable();
            }

            return customObservable;
        }
    }

    // Sobreescribo el metodo porque solo la primera vez que un observador se suscriba,
    // debo crear la BeaconScanner(s) asociados al sector y unirlo de ser el caso para observar
    // varios sectores de una sola vez.
    protected override IDisposable Subscribe(IObserver<ObjectDetected> observer)
    {
        if (beaconsInSectorDetectedObservable == null)
        {
            List<BeaconScanner> scanners = new List<BeaconScanner>();
            foreach(Sector sector in this.implementation.Sectors)
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
            
            beaconsInSectorDetectedObservable.Subscribe((objectDetected) =>
            {
                this.OnObjectDetected(objectDetected);
            });
        }
        
        return Source.Subscribe(observer);
    }
    
    public override void Dispose()
    {
        if (beaconsInSectorDetectedObservable != null)
        {
            beaconsInSectorDetectedObservable.Dispose();
        }
        
        base.Dispose();
    }
}
