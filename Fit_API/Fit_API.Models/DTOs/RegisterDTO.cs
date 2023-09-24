using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class RegisterDTO : LoginDTO
    {
        public string Email { get; set; }
    }
}
