//
// Esta clase implementa un scanner de proximidad via BT con la lib de Estimote
// esta clase recibe un ProximityObserver necesario para realizar al escaneo BT con la lib de estimote
// y sobre la cual se a crear un rango de observacion basado en la distancia, tambien necesita un tag
// requerido por la lib de estimote.
//
public abstract class EstimoteProximityScanner : ProximityScanner<ObjectDetected>
{
    private string tag;
    private readonly ProximityObserver estimoteProximityObserver;
    
    ProximityZone proximityZone;
    IObservable<ObjectDetected> CustomObservable; //crear un observable basado de un subject

    public EstimoteProximityScanner(ProximityObserver estimoteProximityObserver, string tag, Guid id, double distance) : base(id, distance)
    {
        this.estimoteProximityObserver = estimoteProximityObserver;
        this.tag = tag;
    }
    
    // Se sobreescribe el metodo para que solo la primera vez que se suscriban
    // se cree lo necesario para que la lib de estimote escanee objetos.
    public override IDisposable Subscribe(IObserver<T> observer) 
    {
        if (proximityZone == null)
        {
            this.proximityZone = this.estimoteProximityObserver
                                    .zoneBuilder()
                                    .forTag(this.tag)
                                    .inCustomRange(base.distance)
                                    .withOnEnterAction((ProximityContext, Unit) => {
                                        // Por cada objeto detectado, llamo a OnObjectDetecte para que publique el evento
                                        this.OnObjectDetected(EventType.OnEnter, ProximityContext.Id);
                                    })
                                    .withOnExitAction((ProximityContext, Unit) => {    
                                        // Por cada objeto detectado, llamo a OnObjectDetecte para que publique el evento
                                        this.OnObjectDetected(EventType.OnExit, ProximityContext.Id);
                                    })
                                    .withOnChangeAction((ProximityContext[], Unit>) => {
                                        // Por cada objeto detectado, llamo a OnObjectDetecte para que publique el evento
                                        this.OnObjectDetected(EventType.OnChange, ProximityContext[0].Id);
                                    })
                                    .create();
        }
        
        return Source.Subscribe(observer);
    }

    // Se sobreescribe la fuente de eventos, para devolver una fuente real
    // pero solo la primera vez
    protected override IObservable<ObjectDetected> Source
    {
        get
        {
            if (customObservable == null)
            {
                this.customObservable = new CustomObservable();
            }

            return this.customObservable;
        }
    }

    public override void Dispose()
    {
        if (proximityZone != null)
        {
            proximityZone.Dispose();
        }
        
        base.Dispose();
    }
}
