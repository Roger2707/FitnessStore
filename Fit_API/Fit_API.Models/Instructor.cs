﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        // Relation m-m
        public List<ProductInstructor> ProductInstructors { get; set; } = new();
    }
}