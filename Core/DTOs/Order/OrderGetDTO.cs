using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Order
{
    public class OrderGetDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatusEnum status { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress {  get; set; } = string.Empty;
        public double TotalAmount { get; set; }

        public List<OrderItemsDTO> OrderItems { get; set; }

    }
}
