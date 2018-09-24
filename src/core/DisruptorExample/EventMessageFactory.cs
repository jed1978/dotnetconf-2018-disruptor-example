using DisruptorExample.Events;

namespace DisruptorExample
{
    public static class EventMessageFactory
    {
        public static EventMessage GetEventMessage()
        {
            return new EventMessage
            {
                EventType = EventType.Undefined,
            };
        }
    }
}