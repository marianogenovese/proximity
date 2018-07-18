public abstract class EstimoteProximityScanner : ProximityScanner<ObjectDetected>
{
    private string tag;
    private readonly ProximityObserver estimoteProximityObserver;
    
    ProximityZone proximityZone;
    IObservable<ObjectDetected> customObservable; //crear un observable basado de un subject

    public EstimoteProximityScanner(ProximityObserver estimoteProximityObserver, string tag, Guid id, double distance) : base(id, distance)
    {
        this.estimoteProximityObserver = estimoteProximityObserver;
        this.tag = tag;
    }

    public override IObservable<ObjectDetected> Source
    {
        get
        {
            if (proximityZone == null)
            {
                this.proximityZone = this.estimoteProximityObserver
                                        .zoneBuilder()
                                        .forTag(this.tag)
                                        .inCustomRange(base.distance)
                                        .withOnEnterAction((ProximityContext, Unit) => {
                                            this.OnObjectDetected(EventType.OnEnter, ProximityContext.Id);
                                        })
                                        .withOnExitAction((ProximityContext, Unit) => {    
                                            this.OnObjectDetected(EventType.OnExit, ProximityContext.Id);
                                        })
                                        .withOnChangeAction((ProximityContext[], Unit>) => {
                                            this.OnObjectDetected(EventType.OnChange, ProximityContext[0].Id);
                                        })
                                        .create();
                
                this.customObservable = new customObservable();
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
