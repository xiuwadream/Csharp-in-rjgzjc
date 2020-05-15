using System.Collections.Generic;
namespace TodoApi.models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Date { get; set; }
        public string Customer { get; set; }

        public List<Item> Items { get; set; }
    }
}