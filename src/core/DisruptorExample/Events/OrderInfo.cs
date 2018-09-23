namespace DisruptorExample.Events
{
    public class OrderInfo
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public decimal Price { get; set; }
    }
}