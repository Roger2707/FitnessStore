using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models.DTOs
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
