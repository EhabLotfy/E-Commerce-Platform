using Azure;
using Core.Constants;
using Core.Constants.Errors;
using Core.DTOs.Order;
using Core.Helper;
using Core.Helper.OrderState;
using Core.IRepos;
using Foods.Core.Helper;
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
        public IOrderState State { get;internal set; }
        public OrderRepo(DataContext context)
        {
            _context = context;
            State = new OrderProcessingState();
        }  
        
        #region Get
        public APIResponse<OrderGetDTO> GetById(int orderId)
        {
            APIResponse<OrderGetDTO> response = new();
            try
            {
                if (orderId > 0)
                {
                    response.Data = _context.Orders.Include(c => c.Customer)
                                                   .Include(x => x.OrderItems)
                                                   .ThenInclude(p => p.Product)
                                                   .Where(o => o.Id == orderId)
                                                   .FirstOrDefault()
                                                   .Adapt<OrderGetDTO>();
                }
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
                response.Message = ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<PaginatedList<OrderGetDTO>>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize)
        {
            APIResponse<PaginatedList<OrderGetDTO>> response = new();
            try
            {
                if (customerId > 0)
                {
                    var result = await _context.Orders
                        .Include(c=>c.Customer)
                        .Include(items => items.OrderItems)
                        .ThenInclude(p => p.Product)
                        .Where(o => o.CustomerId == customerId).ToListAsync();
                        
                       var map = result.Adapt<IEnumerable<OrderGetDTO>>();

                    response.Data = PaginatedList<OrderGetDTO>.CreateAsync(map, pageNumber, pageSize);
                }
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
                response.Message = ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<PaginatedList<OrderListDTO>>> GetOrdersAsync(int pageNumber, int pageSize)
        {
            APIResponse<PaginatedList<OrderListDTO>> response = new();
            try
            {
                var result = await _context.Orders.ToListAsync();
                                            

                response.Data = PaginatedList<OrderListDTO>.CreateAsync(result.Adapt<IEnumerable<OrderListDTO>>()
                                                                                        , pageNumber, pageSize);
                response.Message = SharedMessages.SUCCESS;
                response.Status = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        } 
        #endregion

        public async Task<APIResponse<OrderGetDTO>> Create(OrderAddDTO dto)
        {
            
            var response = new APIResponse<OrderGetDTO>();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                #region Validation
                if (dto == null || dto.CustomerId == 0 || dto.OrderItems == null)
                {
                    if (dto == null)
                        response.Message = SharedError.FILLREQUIREDFIELDS;
                    if (dto.CustomerId == 0)
                        response.Message = CustomerErrorsContant.CUSTOMERNOTFOUND;
                    if (dto.OrderItems.Count == 0)
                        response.Message = OrderErrorsConstant.ORDERITEMSEMPTY;
                }
                #endregion

                await _context.Orders.AddAsync(dto.Adapt<OrderSet>());
                await _context.SaveChangesAsync();

               await trans.CommitAsync();
                
               response.Status = StatusCodes.Status200OK;
               response.Message = SharedMessages.SUCCESS;
              

            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                response.Status = 500;
                response.Message = ex.InnerException.Message;
            }
            return response;
        }

    

        public void Update(int OrderId, OrderStatus orderStatus)
        {
            
            throw new NotImplementedException();
        }

        public void Cancel(int OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
