using DisruptorExample.Events;

namespace DisruptorExample
{
    public class EventMessageFactory
    {
        public static EventMessage GetEventMessage()
        {
            return new EventMessage
            {
                EventType = EventType.Undefined,
                EventData = new PayloadInfo
                {
                    Order = new OrderInfo(),
                    Product = new ProductInfo()
                }
            };
        }
    }
}