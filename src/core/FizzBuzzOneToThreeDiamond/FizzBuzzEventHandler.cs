using System.Threading;
using Disruptor;

namespace FizzBuzzOneToThreeDiamond
{
    public class FizzBuzzEventHandler : IEventHandler<FizzBuzzEvent>
    {
        private readonly FizzBuzzStep _step;
        private PaddedLong _fizzBuzzCounter;
        private ManualResetEvent _mre;
        private long _iterations;

        public FizzBuzzEventHandler(FizzBuzzStep step)
        {
            _step = step;
        }

        public void OnEvent(FizzBuzzEvent data, long sequence, bool endOfBatch)
        {
            switch (_step)
            {
                case FizzBuzzStep.Fizz:
                    data.Fizz = (data.Value % 3) == 0;
                    break;
                case FizzBuzzStep.Buzz:
                    data.Buzz = (data.Value % 5) == 0;
                    break;

                case FizzBuzzStep.FizzBuzz:
                    if (data.Fizz && data.Buzz)
                    {
                        _fizzBuzzCounter.Value = _fizzBuzzCounter.Value + 1;
                    }
                    break;
            }

            if(sequence == _iterations - 1)
            {
                _mre.Set();
            }
        }

        public void Reset(ManualResetEvent mre, long iterations)
        {
            _iterations = iterations;
            _mre = mre;
        }
    }
}