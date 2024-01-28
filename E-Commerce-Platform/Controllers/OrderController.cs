using Core.DTOs.Order;
using Core.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _repo;

        public OrderController(IOrderRepo repo)
        {
            _repo = repo;
        }

        #region Get
        [HttpGet("[action]/customerId")]
        [Produces(typeof(List<OrderGetDTO>))]
        public async Task<IActionResult> Get(int customerId,int pageNumber,int pageSize)
        {
            return  Ok(await _repo.GetCustomerOrdersAsync(customerId, pageNumber, pageSize));  
        }
        [HttpGet("[action]")]
        [Produces(typeof(OrderGetDTO))]
        public IActionResult GetById(int orderId)
        {
            return  Ok(_repo.GetById(orderId));  
        }
        #endregion

    }
}
