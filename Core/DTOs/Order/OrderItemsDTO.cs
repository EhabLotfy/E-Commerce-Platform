using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Order
{
    public class OrderItemsDTO
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;        
    } 
}
