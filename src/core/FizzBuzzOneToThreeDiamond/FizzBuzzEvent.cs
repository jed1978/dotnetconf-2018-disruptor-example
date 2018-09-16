using System;
using System.Runtime.InteropServices;

namespace FizzBuzzOneToThreeDiamond
{
//    [StructLayout(LayoutKind.Explicit, Size = 192)]
    public sealed class FizzBuzzEvent
    { 
//        [FieldOffset(0)]
        public long Value;

//        [FieldOffset(56)]
        public bool Fizz;

//        [FieldOffset(120)]
        public bool Buzz;

        public static readonly Func<FizzBuzzEvent> EventFactory = () => new FizzBuzzEvent();

        public void Reset()
        {
            Value = 0L;
            Fizz = false;
            Buzz = false;
        }
    }
}