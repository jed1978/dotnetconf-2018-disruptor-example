using Disruptor;
using DisruptorExample.Events;

namespace DisruptorExample
{
    public class Producer
    {
        private readonly RingBuffer<EventMessage> _ringBuffer;

        public Producer(RingBuffer<EventMessage> ringBuffer)
        {
            _ringBuffer = ringBuffer;
        }

        public void OnData(EventMessage message)
        {
            _ringBuffer.PublishEvent(Translator.Instance, message.EventData);
        }

        private class Translator : IEventTranslatorOneArg<EventMessage, PayloadInfo>
        {
            public static readonly Translator Instance = new Translator();

            private Translator(){}
            
            public void TranslateTo(EventMessage @event, long sequence, PayloadInfo payload)
            {
                @event.EventType = EventType.OrderPlaced;
                @event.EventData = payload;
            }
        }
    }
}