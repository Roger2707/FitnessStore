using Fit_API.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class OrderCreateDTO
    {
        public ShippingAddress ShippingAddress { get; set; }
        public bool SaveAddress { get; set; }
    }
}
