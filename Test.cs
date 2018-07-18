ProximityObserver pObserver = new ... (); // estimote observer

Implementation currentImplementation = null;
SectorScanner sectorScanner = null;

// ImplementationScanner =  ahora esta clase va a decir oye tal vez detecte una implementación porque 
// detecte un beacon, entonces se debe preguntar se detecto un beacon de la misma implementacion o de otra??
ImplementationScanner implementationScanner = new ImplementationScanner(pObserver, "DefaultAuparZone", 50);
implementationScanner.throttle(4).Subscribe((beaconDetected) => {
    //Detecto un beacon de alguna implementación, debo calcular si es la misma impl o es otra
    
    IImplementationProvider implProvider = ImplementationProvider.Get();
    Implementation implementation = implProvider.FindByBeaconId(beaconDetected.Id);
    
    //Si es la primera vez, o si son 2 implementaciones distintas.
    if(currentImplementation == null || currentImplementation.Id != implementation.Id)
    {
        currentImplementation = implementation;
        // Libero los recursos asociado al escaneo de sectores
        if (sectorScanner != null)
        {
            sectorScanner.Dispose();
        }

        // SectorScanner = ahora esta clase recibe una implementación para poder detectar todos
        // los sectores asociados a la implementación y poder recibir noficaciones.
        sectorScanner = new SectorScanner(pObserver, implementation);
        sectorScanner.throttle(0.5).Subscribe((sectorDetected) => {
            //Se detecto un sector
        });
    }
})
