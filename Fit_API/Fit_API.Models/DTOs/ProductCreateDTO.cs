using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile? File { get; set; }
        [Range(100, 1000)]
        public double Price { get; set; }
        public int CategoryId { get; set; }
        //public List<int>? InstructorIds { get; set; }
    }
}
