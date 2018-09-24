using System.Runtime.InteropServices;

namespace DisruptorExample.Events
{
    [StructLayout(LayoutKind.Explicit)]
    public struct OrderInfo
    {
        [FieldOffset(0)] 
        public long Id;

        [FieldOffset(8)]
        public long AccountId;
        
        [FieldOffset(16)]
        public decimal Price;
    }
}