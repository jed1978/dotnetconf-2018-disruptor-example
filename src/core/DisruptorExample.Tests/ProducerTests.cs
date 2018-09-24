using System;
using System.Reflection;
using Disruptor;
using DisruptorExample;
using DisruptorExample.Events;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPublishSuccess()
        {
            var ringBuffer = new RingBuffer<EventMessage>(() => new EventMessage(),
                new SingleProducerSequencer(8, new YieldingWaitStrategy()));
            
            var target = new Producer(ringBuffer);
            
            var message = EventMessageFactory.GetEventMessage();
            message.EventType = EventType.OrderPlaced;
            message.EventData.Order.Id = GetOrderId();
            message.EventData.Order.AccountId = 100001L;
            message.EventData.Order.Price = 100m;

            target.OnData(message);
            Assert.That(ringBuffer.ClaimAndGetPreallocated(0).EventData.Order.Id, Is.EqualTo(message.EventData.Order.Id));
        }

        private long GetOrderId()
        {
            return DateTime.UtcNow.Ticks;
        }
    }
}