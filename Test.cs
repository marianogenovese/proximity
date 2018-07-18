ProximityObserver pObserver = new ... (); // estimote observer

Implementation currentImplementation = null;
SectorScanner sectorScanner = null;

ImplementationScanner implementationScanner = new ImplementationScanner(pObserver, "DefaultAuparZone", 50);
implementationScanner.throttle(4).Subscribe((beaconDetected) => {
    //Detecto un beacon de alguna implementación, debo calcular si es la misma impl o es otra
    
    IImplementationProvider implProvider = ImplementationProvider.Get();
    Implementation implementation = implProvider.FindByBeaconId(beaconDetected.Id);
    
    if(currentImplementation == null || currentImplementation.Id != implementation.Id)
    {  
        currentImplementation = implementation;
        if (sectorScanner != null)
        {
            sectorScanner.Dispose();
        }

        sectorScanner = new SectorScanner(pObserver, implementation);
        sectorScanner.throttle(0.5).Subscribe((sectorDetected) => {
            //Se detecto un sector
        });
    }
})