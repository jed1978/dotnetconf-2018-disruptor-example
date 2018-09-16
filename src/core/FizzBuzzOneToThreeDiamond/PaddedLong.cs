using System.Runtime.InteropServices;

namespace FizzBuzzOneToThreeDiamond
{
    [StructLayout(LayoutKind.Explicit, Size = 72)]
    public struct PaddedLong
    {
        // padding: 12 (java object header)
        // padding: 4 (java padding)

        [FieldOffset(16)]
        public long Value;

        // padding: 46
    }
}