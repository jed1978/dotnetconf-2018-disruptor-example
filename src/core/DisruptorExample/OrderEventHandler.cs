using Disruptor;
using DisruptorExample.Events;

namespace DisruptorExample
{
    public class OrderEventHandler: IEventHandler<EventMessage>
    {
        public void OnEvent(EventMessage data, long sequence, bool endOfBatch)
        {
            if (data.EventType == EventType.OrderPlaced)
            {
                OnOrderPlaced(ref data.EventData.Order);
            }
        }

        private void OnOrderPlaced(ref OrderInfo orderInfo)
        {
            //do OrderPlaced logic here
        }
    }
}