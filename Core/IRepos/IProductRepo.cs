namespace Core.IRepos
{
    public interface IProductRepo
    {
        bool IsAvailableInStock (int productId , int quantity);
        bool UpdateProductQuantity(int productId , int quantity);
    }
}
