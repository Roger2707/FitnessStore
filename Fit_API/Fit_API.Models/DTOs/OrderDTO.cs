using Fit_API.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public double SubTotal { get; set; }
        public double DeliveryFee { get; set; }
        public string OrderStatus { get; set; }
        public double Total { get; set; }
        public List<OrderItemDTO> OrderItemsDTO { get; set; }
    }
}
