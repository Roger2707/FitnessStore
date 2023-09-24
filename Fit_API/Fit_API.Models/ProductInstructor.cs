using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models
{
    public class ProductInstructor
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? InstructorId { get; set; }
        public Product? Product { get; set; }
        public Instructor? Instructor { get; set; }
    }
}
