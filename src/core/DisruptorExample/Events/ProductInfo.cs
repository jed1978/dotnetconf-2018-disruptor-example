using System.Runtime.InteropServices;

namespace DisruptorExample.Events
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ProductInfo
    {
        [FieldOffset(0)]
        public long Id;
        
    }
}