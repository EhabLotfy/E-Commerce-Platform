
namespace Core.Models
{
    [Table(name: "Product", Schema = "Main")]
    public class ProductSet : BaseEntity
    {
        #region Properties

        [Required(ErrorMessage = "Description is required!"), MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage ="Description is required!")]
        public string Description { get; set; } = string.Empty;

        [Range(0,int.MaxValue,ErrorMessage ="Price cannot be less than 0")]
        public double Price { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        #endregion

    }
}
