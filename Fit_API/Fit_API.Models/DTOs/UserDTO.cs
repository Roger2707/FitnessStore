using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public ShoppingCartDTO ShoppingCart { get; set; }
    }
}
