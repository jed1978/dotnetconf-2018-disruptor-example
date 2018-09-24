using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using DisruptorExample.Events;

namespace DisruptorExample
{
    public class OrderService
    {
        public Disruptor<EventMessage> Disruptor { get; }

        public bool IsRunning { get; private set; }
        
        private readonly ManualResetEvent _latch;
        private int _bufferSize = 1024;
        private readonly RingBuffer<EventMessage> _ringBuffer;
        private IBatchEventProcessor<EventMessage> _orderBatchProcessor;
        private readonly List<Task> _tasks = new List<Task>();

        public OrderService()
        {
            _latch = new ManualResetEvent(false);
            Disruptor = new Disruptor<EventMessage>(
                EventMessageFactory.GetEventMessage,
                _bufferSize, 
                TaskScheduler.Default, 
                ProducerType.Single, 
                new YieldingWaitStrategy());
            _ringBuffer = Disruptor.RingBuffer;
        }
        
        public void Run()
        {
            if (IsRunning) return;
            
            var sequenceBarrier = _ringBuffer.NewBarrier();
            var orderEventHandler = new OrderEventHandler();

            _orderBatchProcessor = BatchEventProcessorFactory.Create(_ringBuffer, sequenceBarrier, orderEventHandler);
            _orderBatchProcessor.WaitUntilStarted(TimeSpan.FromSeconds(5));
            _tasks.Add(Task.Run(() => _orderBatchProcessor.Run()));

            IsRunning = true;
            _latch.WaitOne();
            _orderBatchProcessor.Halt();
            Task.WaitAll(_tasks.ToArray());
            
            IsRunning = false;
        }

        public void Stop()
        {
            _latch.Set();
            
        }
    }
}