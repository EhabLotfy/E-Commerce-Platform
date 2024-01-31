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
        Task<APIResponse<PaginatedList<OrderGetDTO>>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize);
        Task<APIResponse<PaginatedList<OrderListDTO>>> GetOrdersAsync(int pageNumber, int pageSize);
        APIResponse<OrderGetDTO> GetById(int orderId);
        Task<APIResponse<object>> Create(OrderAddDTO dto);
        void Update(int OrderId, OrderStatus orderStatus);
        APIResponse<object> Cancel(int OrderId);

    }
}
