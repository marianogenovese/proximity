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

    // Source sirve como fuente de eventos privado, a Source es donde se van a publicar eventos,
    // y los observadores se van a subscribir.
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
    
    // Este es el metodo que van a llamar los observadores,
    // por lo que en lugar de suscribirse a ProximityScanner, se suscriben a Source
    // que es la fuente de eventos.
    public virtual IDisposable Subscribe(IObserver<T> observer) 
    {
        return Source.Subscribe(observer);
    }

    // Valida que el objeto detectado, pertenezca a los objetos relacionados
    protected virtual bool Validate(EventType type, Guid objectId)
    {
        return this.ReleatedObjects.any(objectId);
    }

    // Ayuda para publicar eventos de objetos detectados a la fuente de eventos
    protected virtual void OnObjectDetected(EventType type, Guid objectId)
    {
        if(Validate(type, objectId))
        {
            Source.onNext(new ObjectDetected(objectId, type));
        }
    }
    
    // Libera recursos
    public virtual void Dispose()
    {
        if (this.Source != null)
        {
            // Al hacer esto, todos los observadores asociados a la fuente
            // terminan de observar.
            this.Source.Dispose();
        }
    }
}
