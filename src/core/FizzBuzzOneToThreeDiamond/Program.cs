using System;
using System.Diagnostics;

namespace FizzBuzzOneToThreeDiamond
{
    class Program
    {
        static void Main(string[] args)
        {
            var iteration = 1000 * 1000 * 1000;
            var bufferSize = 1024 * 32;
            var processorCount = 3;
            var fizzBuzzSequenceTest = new FizzBuzzSequenceTest(processorCount, iteration, bufferSize);

            var sw = new Stopwatch();
            fizzBuzzSequenceTest.Run(sw);
            var elapsedSecs = sw.Elapsed.Seconds;

            Console.WriteLine(
                $"FizzBuzzSequenceTest, executed {iteration} iterations, elapsed {elapsedSecs} secs, ops: {iteration / elapsedSecs}");

            Console.Read();
        }
    }
}
