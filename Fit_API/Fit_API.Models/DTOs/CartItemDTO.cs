using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public List<string> InstructorsName { get; set; }
    }
}
