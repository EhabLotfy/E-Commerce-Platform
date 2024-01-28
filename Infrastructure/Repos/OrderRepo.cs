using Core.DTOs.Order;
using Core.Helper;
using Core.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DataContext _context;

        public OrderRepo(DataContext context)
        {
            _context = context;
        }
        public void Cancel(int OrderId)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse<OrderGetDTO>> Create(OrderAddDTO dto)
        {
            
            var response = new APIResponse<OrderGetDTO>();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                response.Status = 500;
                response.Message = ex.InnerException.Message;
            }
            return response;
        }

        public Task<OrderGetDTO> GetAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderGetDTO>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderGetDTO>> GetOrdersAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Update(int OrderId, OrderStatusEnum orderStatus)
        {
            throw new NotImplementedException();
        }
    }
}
