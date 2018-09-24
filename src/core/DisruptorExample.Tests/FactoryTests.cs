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
            });
        }
    }
}