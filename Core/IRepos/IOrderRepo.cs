using Core.DTOs.Order;
using Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepos
{
    public interface IOrderRepo
    {
        Task<List<OrderGetDTO>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize);
        Task<List<OrderGetDTO>> GetOrdersAsync(int pageNumber, int pageSize);
        Task<OrderGetDTO> GetAsync(int orderId);
        Task<APIResponse<OrderGetDTO>> Create(OrderAddDTO dto);
        void Update(int OrderId, OrderStatusEnum orderStatus);
        void Cancel(int OrderId);

    }
}
