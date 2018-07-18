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

    public void Dispose()
    {
        if (this.customObservable != null)
        {
            customObservable.Dispose();
        }
    }

    protected override IDisposable Subscribe(IObserver<ObjectDetected> observer)
    {
        if (sectorScanners == null)
        {
            List<BeaconScanner> scanners = new List<BeaconScanner>();
            foreach(Sector sector in this.implementation.Sectors)
            {
                scanners.Add(new BeaconScanner(this.estimoteProximityObserver, "CustomAuparAzone", sector))
            }
            
            BeaconScanner[] beconScanners = scanners.ToArray();

            foreach(BeaconScanner scanner in beconScanners)
            {
                if (beaconsInSectorDetectedObservable == null)
                {
                    beaconsInSectorDetectedObservable = scanner;
                }
                else
                {                    
                    beaconsInSectorDetectedObservable = beaconsInSectorDetectedObservable.Merge(scanner);
                }
            }

            beaconsInSectorDetectedObservable.Subscribe((objectDetected) =>
            {
                this.OnObjectDetected(objectDetected.Type, objectDetected.Id);
            });
        }
        
        return Source.Subscribe(observer);
    }
