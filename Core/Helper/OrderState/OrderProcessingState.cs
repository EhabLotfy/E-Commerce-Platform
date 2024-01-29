using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper.OrderState
{
    public class OrderProcessingState : IOrderState
    {
        public void Processing()
        {
            // Must Implement this method
            throw new NotImplementedException();
        }
        public void Delivered()
        {
            throw new NotImplementedException();
        }
        public void Shipped()
        {
            throw new NotImplementedException();
        }
    }
}
