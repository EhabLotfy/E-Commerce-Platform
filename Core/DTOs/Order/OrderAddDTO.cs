using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Order
{
    public class OrderAddDTO
    {
        public int CustomerId { get; set; }
        public List<OrderItemsAddDTO> OrderItems { get; set; }
    }
}
