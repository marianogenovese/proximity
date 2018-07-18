public abstract class ProximityScanner<T> : IObservable, IDisposable where T : ObjectDetected
{
    private readonly Guid id;
    private readonly double distance;

    public ProximityScanner(Guid id, double distance)
    {
        this.id = id;
        this.distance = distance;
    }

    public Guid Id
    {
        get
        {
            return this.id;
        }
    }

    public abstract Guid[] ReleatedObjects
    {
        get;
    }

    public abstract Observable<T> Source
    {
        get;
    }

    public virtual double Distance
    {
        get
        {
            return this.distance;
        }
    }
    
    public virtual IDisposable Subscribe(IObserver<T> observer) 
    {
        return Source.Subscribe(observer);
    }

    protected virtual bool Validate(EventType type, Guid objectId)
    {
        return this.ReleatedObjects.any(objectId);
    }

    protected virtual void OnObjectDetected(EventType type, Guid objectId)
    {
        if(Validate(type, objectId))
        {
            Source.onNext(new ObjectDetected(objectId, type));
        }
    }
}