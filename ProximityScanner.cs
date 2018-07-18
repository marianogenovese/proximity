//
// Clase base de todas las clases que deseen detectar proximidad y tengan objetos relacionados
// que desean escanear. Esta clase es Observable para poder recibir eventos via patron observador/observable
// La proximidad necesita: un Id y una distancia como mínimo.
//
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

    // Todas las clases que hereden de esta, deben declarar cuales son
    // sus objetos relacionados
    public abstract Guid[] ReleatedObjects
    {
        get;
    }

    protected abstract Observable<T> Source
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
    
    public virtual void Dispose()
    {
        if (this.Source != null)
        {
            this.Source.Dispose();
        }
    }
}
