using System;

namespace testDynamicZoneLib
{
    public class ObjectDetected
    {
        private readonly Guid id;
        private readonly EventType type;

        public ObjectDetected(Guid id, EventType type)
        {
            this.id = id;
            this.type = type;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public EventType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}
