using Core.Constants;
using Core.DTOs.Order;
using Core.Helper;
using Core.IRepos;
using Mapster;
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

        public APIResponse<OrderGetDTO> GetById(int orderId)
        {
            APIResponse<OrderGetDTO> response = new();
            try
            {
                response.Data = _context.Orders.Include(c => c.Customer)
                                               .Include(x=>x.OrderItems)
                                               .ThenInclude(p=>p.Product)
                    .Where(o=> o.Id == orderId).Select(
                    order => new OrderGetDTO
                    {
                        Id = order.Id,
                        OrderDate = order.OrderDate,
                        CustomerId = order.CustomerId,
                        CustomerName = order.Customer.Name,
                        CustomerEmail = order.Customer.Email,
                        CustomerAddress = order.Customer.Address,
                        TotalAmount = order.TotalAmount,
                        status = order.status,
                        OrderItems = order.OrderItems.ToList().Adapt<List<OrderItemsDTO>>()
                       
                    }).FirstOrDefault().Adapt<OrderGetDTO>();

                if (response.Data == null)
                {
                    response.Message = OrderConstants.ORDERNOTFOUND;
                    response.Status = StatusCodes.Status404NotFound;
                    return response;
                }

                response.Message = SharedMessages.SUCCESS;
                response.Status = StatusCodes.Status200OK;              
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public Task<APIResponse<List<OrderGetDTO>>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<List<OrderGetDTO>>> GetOrdersAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Update(int OrderId, OrderStatusEnum orderStatus)
        {
            throw new NotImplementedException();
        }
    }
}
