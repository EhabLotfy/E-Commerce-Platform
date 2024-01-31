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
        private readonly IProductRepo _productRepo;

        public IOrderState State { get;internal set; }
        public OrderRepo(DataContext context,IProductRepo productRepo)
        {
            _context = context;
            _productRepo = productRepo;
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

        public async Task<APIResponse<object>> Create(OrderAddDTO dto)
        {
            
            var response = new APIResponse<object>();
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

                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                #endregion
                double TotalAmount = await GetOrderTotalAsync(dto.OrderItems);

                OrderSet orderSet = new OrderSet()
                {
                    CustomerId = dto.CustomerId,
                    TotalAmount = TotalAmount,
                    status = OrderStatus.Pending
                };

                await _context.Orders.AddAsync(orderSet);
                await _context.SaveChangesAsync();

                int orderId = orderSet.Id;
                if (orderId > 0)
                {
                    var orderItems = FillOrderItems(dto.OrderItems, orderId).Adapt<List<OrderItemsSet>>();

                    foreach (var item in orderItems)
                    {
                        if (item.Quantity == 0)
                        {
                            await trans.RollbackAsync();
                            response.Status = StatusCodes.Status400BadRequest;
                            response.Message = $"Product with ID {item.ProductId} not in stock please remove it from product list!";                            
                            return response;
                        }

                        _productRepo.UpdateProductQuantity(item.ProductId,item.Quantity,false);
                    }
                    
                    orderSet.OrderItems = orderItems;
                }
                else
                {
                    await trans.RollbackAsync();
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Message = OrderErrorsConstant.ORDERNOTSAVED;
                    return response;
                }

                await _context.SaveChangesAsync();

                await trans.CommitAsync();


                response.Status = StatusCodes.Status200OK;
               response.Message = SharedMessages.SUCCESS;
                response.Data = orderSet.Adapt<OrderGetDTO>();

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

        public async Task<APIResponse<object>> Cancel(int OrderId)
        {
            APIResponse<object> response = new();
            var trans = _context.Database.BeginTransaction();
            try
            {
                // validate orederId
                var order =await _context.Orders.Include(o => o.OrderItems)
                                                    .ThenInclude(p => p.Product)
                                                    .Where(order=>order.Id==OrderId)
                                                    .AsSplitQuery()
                                                    .FirstOrDefaultAsync();
              
                if(order is  null)
                {
                    await trans.RollbackAsync();
                    response.Message = "Order not found!";
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                if (order.status != OrderStatus.Pending)
                {
                    await trans.RollbackAsync();
                    response.Message = "Fail: Order not canceled you can cancel this order if his status is pending only!";
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                
                order.status = OrderStatus.Canceled;
                _context.Orders.Update(order);
                _context.SaveChanges();
                
                foreach( var item in order.OrderItems)
                {
                    _productRepo.UpdateProductQuantity(item.ProductId,item.Quantity,true);                   
                }

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

        #region HelperMethods
        private IEnumerable<object> FillOrderItems(IEnumerable<OrderItemsAddDTO> orderItems, int orderId)
        {
            var orderItemsList = new List<OrderItemsSet>();
            var notInStockProductsList = new List<OrderItemsSet>();
            foreach (var item in orderItems)
            {
               // Check product and quantity
               var IsItemExist= _productRepo.IsAvailableInStock(item.ProductId, item.Quantity);

                if (IsItemExist)
                {
                    orderItemsList.Add(new OrderItemsSet
                    {
                        Quantity = item.Quantity,
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Price = _context.Products.First(p => p.Id == item.ProductId).Price * item.Quantity,
                    });
                }
                else
                {
                    notInStockProductsList.Add(new OrderItemsSet
                    {
                        Quantity = 0,
                        ProductId = item.ProductId
                    }) ;                                                         
                }
            }
            return orderItemsList.Concat(notInStockProductsList);
        }

        private async Task<double> GetOrderTotalAsync(IEnumerable<OrderItemsAddDTO> products)
        {
            var total = 0.0;
            var productsIds = products.Select(e => e.ProductId).ToList();
            var productsList = await _context.Products.Where(e => productsIds.Contains(e.Id))
                .Select(e => new
                {
                    ProductId = e.Id,
                    Price = e.Price
                }).ToListAsync();

            foreach ( var product in products )
            {
                var OrignalPrice = productsList.Where(e => e.ProductId == product.ProductId).
                    Select(e => e.Price).FirstOrDefault();

                    total += OrignalPrice * product.Quantity;
            }
            

            return total;
        }
        #endregion
    }
}
