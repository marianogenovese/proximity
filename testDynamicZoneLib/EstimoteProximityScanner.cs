using System;
using System.Reactive.Subjects;

namespace testDynamicZoneLib
{
    public abstract class EstimoteProximityScanner : ProximityScanner<ObjectDetected>
    {
        private string tag;
        private readonly ProximityObserver estimoteProximityObserver;

        ProximityZone proximityZone;
        ProximityObserverHandler proximityHandler;
        Subject<ObjectDetected> customObservable; //crear un observable basado de un subject

        public EstimoteProximityScanner(ProximityObserver estimoteProximityObserver, string tag, Guid id, double distance) : base(id, distance)
        {
            this.estimoteProximityObserver = estimoteProximityObserver;
            this.tag = tag;
        }

        // Se sobreescribe el metodo para que solo la primera vez que se suscriban
        // se cree lo necesario para que la lib de estimote escanee objetos.
        public override IDisposable Subscribe(IObserver<ObjectDetected> observer)
        {
            if (proximityZone == null)
            {
                /*
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
                                        .withOnChangeAction((ProximityContext[], Unit >) => {
                    // Por cada objeto detectado, llamo a OnObjectDetecte para que publique el evento
                    this.OnObjectDetected(EventType.OnChange, ProximityContext[0].Id);
                })
                                    .create();

                // Agrego la zona creada a estimote y comienzo a observar.
                this.proximityHandler = estimoteProximityObserver.addProximityZone(this.proximityZone).Start();
                */
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
                    this.customObservable = new Subject<ObjectDetected>();
                }

                return this.customObservable;
            }
        }

        protected override void PublishEvent(ObjectDetected objectDetected)
        {
            this.customObservable.OnNext(objectDetected);
        }

        public override void Dispose()
        {
            if (proximityHandler != null)
            {
                proximityHandler.Stop();
                proximityHandler.Dispose();
            }

            if (proximityZone != null)
            {
                proximityZone.Dispose();
            }

            if (this.customObservable != null)
            {
                this.customObservable.Dispose();
            }
        }
    }
}
