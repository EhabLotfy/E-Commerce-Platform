using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Order
{
    public class OrderListDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus status { get; set; }
        public double TotalAmount { get; set; }


    }
}
