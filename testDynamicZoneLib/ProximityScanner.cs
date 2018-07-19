using System;
using System.Linq;

namespace testDynamicZoneLib
{
    public abstract class ProximityScanner<T> : IObservable<T>, IDisposable where T : ObjectDetected
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
        protected abstract Guid[] ReleatedObjects
        {
            get;
        }

        // Source sirve como fuente de eventos privado, a Source es donde se van a publicar eventos,
        // y los observadores se van a subscribir.
        protected abstract IObservable<T> Source
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
        protected virtual bool Validate(T objectDetected)
        {
            return this.ReleatedObjects.Any(x => x.Equals(objectDetected.Id));
        }

        // Ayuda para publicar eventos de objetos detectados a la fuente de eventos
        protected virtual void OnObjectDetected(T objectDetected)
        {
            if (Validate(objectDetected))
            {
                PublishEvent(objectDetected);
            }
        }

        protected abstract void PublishEvent(T objectDetected);

        // Libera recursos
        public abstract void Dispose();
    }
}
