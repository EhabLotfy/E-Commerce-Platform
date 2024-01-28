namespace Core.Models
{
    [Table(name: "Customer", Schema = "Main")]
    public class CustomerSet : BaseEntity
    {
        #region Properties
        
        [Required , MaxLength(100)]
        public string Name { get; set; }
        
        [Required ,EmailAddress,MaxLength(200)]
        public string Email { get; set; } 

        [Required,MaxLength(200)]
        public string Address { get; set; } =string.Empty;
        #endregion

        #region Navigation properties
        public ICollection<OrderSet> Orders { get; set; }
        #endregion
    }  
}
