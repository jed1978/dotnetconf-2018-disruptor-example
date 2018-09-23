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
            var message = new EventMessage
            {
                EventType = EventType.OrderPlaced,
                EventData = new PayloadInfo
                {
                    Order = new OrderInfo
                    {
                        Id = GetOrderId(),
                        AccountId = 100001L,
                        Price = 100m
                    }
                }
            };

            target.OnData(message);
            Assert.That(ringBuffer.ClaimAndGetPreallocated(0).EventData.Order.Id, Is.EqualTo(message.EventData.Order.Id));
        }

        private long GetOrderId()
        {
            return DateTime.UtcNow.Ticks;
        }
    }
}