using Core.DTOs.Order;
using Core.Helper;
using Foods.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepos
{
    public interface IOrderRepo
    {
        Task<object> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize);
        Task<object> GetOrdersAsync(int pageNumber, int pageSize);
        object GetById(int orderId);
        Task<object> Create(OrderAddDTO dto);
        Task<APIResponse<object>> UpdateState(int OrderId, OrderStatus orderStatus);
        Task<object> Cancel(int OrderId);

    }
}
