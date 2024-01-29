using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper.OrderState
{
    public interface IOrderState
    {
        void Processing();
        void Shipped();
        void Delivered();

    }
}
