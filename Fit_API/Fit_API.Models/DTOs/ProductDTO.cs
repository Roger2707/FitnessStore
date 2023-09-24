using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }

        // Category
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // Instructor
        public List<Instructor>? Instructors { get; set; }
    }
}
