// Esta clase por debajo usa BT para detectar cualquier objecto (beacon) sin importar
// que este relacionado o no, basicamente necesitamos que esta clase inicie todo el proceso de 
// deteccion de objetos y cambios.
public class ImplementationScanner : EstimoteProximityScanner
{
    public ImplementationScanner(ProximityObserver estimoteProximityObserver, string tag, double distance) : base(estimoteProximityObserver, tag, Guid.Empty, distance)
    {
        this.distance = distance;
    }

    protected override Guid[] ReleatedObjects
    {
        get
        {
            return Enumerable.Empty<Guid>();
        }
    }

    protected override bool Validate(EventType type, Guid id)
    {
        //Para que cualquier objecto que detecte genere un evento
        return true;
    }
}
