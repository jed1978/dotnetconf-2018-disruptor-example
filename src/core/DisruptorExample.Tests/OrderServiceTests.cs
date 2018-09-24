using System;
using System.Threading.Tasks;
using DisruptorExample;
using DisruptorExample.Events;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public void TestOrderServiceSuccessRunAndStop()
        {
            var service = new OrderService();
            var producer = new Producer(service.Disruptor.RingBuffer);

            Task.Run(()=> service.Run());

            for (int i = 0; i < 10; i++)
            {
                var msg = EventMessageFactory.GetEventMessage();
                msg.EventType = EventType.OrderPlaced;
                msg.EventData.Order.Id = i;
                msg.EventData.Order.Price = 100;
                msg.EventData.Order.AccountId = 1000001;
                
                producer.OnData(msg);
            }
            
            service.Stop();

            var actual = service.Disruptor.RingBuffer.ClaimAndGetPreallocated(9);
            Assert.Multiple(()=>
            {
                Assert.That(actual.EventType, Is.EqualTo(EventType.OrderPlaced));
                Assert.That(actual.EventData.Order.Id, Is.EqualTo(9));
                Assert.That(actual.EventData.Order.Price, Is.EqualTo(100));
            });
        }
    }
}