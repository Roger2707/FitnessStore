using Fit_API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}
