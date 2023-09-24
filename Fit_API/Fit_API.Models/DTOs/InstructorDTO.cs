using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class InstructorDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public List<ProductDTO>? Products { get; set; }
    }
}
