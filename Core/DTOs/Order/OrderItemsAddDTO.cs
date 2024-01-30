using System.Text.Json.Serialization;

namespace Core.DTOs.Order
{
    public class OrderItemsAddDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public int OrderId { get; set; }

        [JsonIgnore]
        public double Price { get; set; }
 
    }
}
