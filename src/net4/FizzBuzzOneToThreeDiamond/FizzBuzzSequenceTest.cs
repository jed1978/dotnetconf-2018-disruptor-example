using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Disruptor;

namespace FizzBuzzOneToThreeDiamond
{
    public class FizzBuzzSequenceTest
    {
        private readonly long _iterations; // = 1000L * 1000L * 300L;
        private readonly int _processorCount;
        private readonly int _bufferSize; // = 1024 * 8;
        
        private readonly long _expectedResult;

        private readonly RingBuffer<FizzBuzzEvent> _ringBuffer;
        private readonly BatchEventProcessor<FizzBuzzEvent> _batchProcessorFizz;
        private readonly BatchEventProcessor<FizzBuzzEvent> _batchProcessorBuzz;
        private readonly BatchEventProcessor<FizzBuzzEvent> _batchProcessorFizzBuzz;
        private readonly FizzBuzzEventHandler _fizzBuzzHandler;


        public FizzBuzzSequenceTest(int processorCount, long iterations, int bufferSize)
        {
            _processorCount = processorCount;
            _iterations = iterations;
            _bufferSize = bufferSize;
            _ringBuffer = RingBuffer<FizzBuzzEvent>.CreateSingleProducer(FizzBuzzEvent.EventFactory, _bufferSize, new YieldingWaitStrategy());

            var sequenceBarrier = _ringBuffer.NewBarrier();

            var fizzHandler = new FizzBuzzEventHandler(FizzBuzzStep.Fizz);
            _batchProcessorFizz = new BatchEventProcessor<FizzBuzzEvent>(_ringBuffer, sequenceBarrier, fizzHandler);

            var buzzHandler = new FizzBuzzEventHandler(FizzBuzzStep.Buzz);
            _batchProcessorBuzz = new BatchEventProcessor<FizzBuzzEvent>(_ringBuffer, sequenceBarrier, buzzHandler);

            var sequenceBarrierFizzBuzz = _ringBuffer.NewBarrier(_batchProcessorFizz.Sequence, _batchProcessorBuzz.Sequence);

            _fizzBuzzHandler = new FizzBuzzEventHandler(FizzBuzzStep.FizzBuzz);
            _batchProcessorFizzBuzz = new BatchEventProcessor<FizzBuzzEvent>(_ringBuffer, sequenceBarrierFizzBuzz, _fizzBuzzHandler);

            var temp = 0L;
            for (long i = 0; i < _iterations; i++)
            {
                var fizz = 0 == (i % 3L);
                var buzz = 0 == (i % 5L);

                if (fizz && buzz)
                {
                    ++temp;
                }
            }
            _expectedResult = temp;

            _ringBuffer.AddGatingSequences(_batchProcessorFizzBuzz.Sequence);
        }

        public void Run(Stopwatch sw)
        {
            var mre = new ManualResetEvent(false);
            _fizzBuzzHandler.Reset(mre, _batchProcessorFizzBuzz.Sequence.Value + _iterations);

            var processorTask1 = Task.Run(() => _batchProcessorFizz.Run());
            var processorTask2 = Task.Run(() => _batchProcessorBuzz.Run());
            var processorTask3 = Task.Run(() => _batchProcessorFizzBuzz.Run());



            _batchProcessorFizz.WaitUntilStarted(TimeSpan.FromSeconds(5));
            _batchProcessorBuzz.WaitUntilStarted(TimeSpan.FromSeconds(5));
            _batchProcessorFizzBuzz.WaitUntilStarted(TimeSpan.FromSeconds(5));

            sw.Start();
            for (long i = 0; i < _iterations; i++)
            {
                var sequence = _ringBuffer.Next();
                _ringBuffer[sequence].Value = i;
                _ringBuffer.Publish(sequence);
            }

            mre.WaitOne();
            sw.Stop();
            _batchProcessorFizz.Halt();
            _batchProcessorBuzz.Halt();
            _batchProcessorFizzBuzz.Halt();
            Task.WaitAll(processorTask1, processorTask2, processorTask3);
        }
    }
}