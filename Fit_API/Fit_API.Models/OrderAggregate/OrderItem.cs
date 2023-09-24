using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.OrderAggregate
{
    public class OrderItem
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOrdered ProductItemOrdered { get; set; }
    }
}
