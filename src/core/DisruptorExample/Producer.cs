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
            _ringBuffer.PublishEvent(Translator.Instance, message.EventType, message.EventData);
        }

        private class Translator : IEventTranslatorTwoArg<EventMessage, EventType, PayloadInfo>
        {
            public static readonly Translator Instance = new Translator();

            private Translator(){}
            
            public void TranslateTo(EventMessage @event, long sequence, EventType eventType, PayloadInfo payload)
            {
                @event.EventType = eventType;
                if (eventType == EventType.OrderPlaced)
                {
                    @event.EventData.Order.Id = payload.Order.Id;
                    @event.EventData.Order.AccountId = payload.Order.AccountId;
                    @event.EventData.Order.Price = payload.Order.Price;
                }
            }
        }
    }
}