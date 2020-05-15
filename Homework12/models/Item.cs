namespace TodoApi.models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Good { get; set; }
        public int Count { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}