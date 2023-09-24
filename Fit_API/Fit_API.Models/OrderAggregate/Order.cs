using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.OrderAggregate
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public double Subtotal { get; set; }
        public double DeliveryFee { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public ShippingAddress ShippingAddress { get; set; }

        public List<OrderItem> Items { get; set; }

        public double GetTotal()
        {
            return Subtotal + DeliveryFee;
        }
    }
}
