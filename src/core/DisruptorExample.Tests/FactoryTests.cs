using DisruptorExample;
using DisruptorExample.Events;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FactoryTests
    {
        [Test]
        public void TestGetEventMessage()
        {
            
            var target = EventMessageFactory.GetEventMessage();

            Assert.Multiple(() =>
            {
                Assert.That(target, Is.Not.Null, "EventMessage must not be null");
                Assert.That(target.EventType, Is.EqualTo(EventType.Undefined));
                Assert.That(target.EventData, Is.Not.Null.Or.Empty, "Event Payload must not be empty");
                Assert.That(target.EventData.Order, Is.Not.Null, "The OrderInfo must not be null");
            });
        }
    }
}