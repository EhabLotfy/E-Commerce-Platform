
namespace Core.Models
{
    [Table(name: "Order", Schema = "Main")]
    public class OrderSet : BaseEntity
    {
        #region Constructor
        public OrderSet()
        {
            OrderDate = DateTime.UtcNow;
          
        } 
        #endregion

        #region Properties        
        public DateTime OrderDate { get; set; }

        [Range(0, int.MaxValue)] 
        public double TotalAmount { get; set; }
        public OrderStatus status { get; set; }
        #endregion

        #region Navigation properties
        public int CustomerId { get; set; }
        public CustomerSet Customer { get; set; }

        public  ICollection<OrderItemsSet> OrderItems { get; set; }
        #endregion
    }
}
