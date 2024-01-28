namespace Core.Models
{

    [Table(name:"OrderItems", Schema ="Main")]

    public class OrderItemsSet
    {
        #region Properties
        [Range(0, int.MaxValue)] public int Quantity { get; set; }
        [Range(0, int.MaxValue)] public double Price { get; set; }
        #endregion

        #region Navigation properties
        public int OrderId { get; set; }
        public OrderSet Order { get; set; }

        public int ProductId { get; set; }
        public ProductSet Product { get; set; }

        #endregion
    }
}
