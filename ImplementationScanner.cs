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