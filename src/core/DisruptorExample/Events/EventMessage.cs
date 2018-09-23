using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DisruptorExample.Events
{
    public class EventMessage
    {
        public EventType EventType;
        public PayloadInfo EventData;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PayloadInfo
    {
        [FieldOffset(0)]
        public OrderInfo Order;

        [FieldOffset(0)]
        public ProductInfo Product;
    }
}