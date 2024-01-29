namespace Core.Helper.OrderState
{
    public class OrderShippedState : IOrderState
    {
        public void Shipped()
        {
            // Must Implement this method
            OrderStatus orderStatus = OrderStatus.Shipped;
        }
        public void Processing()
        {
            throw new NotImplementedException();
        }
        public void Delivered()
        {
            throw new NotImplementedException();
        }
    }
}
