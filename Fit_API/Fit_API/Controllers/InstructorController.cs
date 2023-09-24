using Fit_API.DataAccess.Data;
using Fit_API.Models;
using Fit_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : BaseAPIController
    {
        private readonly ApplicationDbContext _db;
        public InstructorController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var instructors = await _db.Instructors.Include(i => i.ProductInstructors)
                .ThenInclude(p => p.Product).ThenInclude(p => p.Category)
                .Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Products = i.ProductInstructors.Select(p => 
                    new ProductDTO {
                        Id = (int)p.ProductId,
                        Name = p.Product.Name,
                        Description = p.Product.Description,
                        Price = p.Product.Price,
                        ImageUrl = p.Product.ImageUrl,
                        CategoryId = p.Product.CategoryId,
                        CategoryName = p.Product.Category.Name,
                    }).ToList(),
                }).ToListAsync();
            if (instructors == null) return BadRequest(new ProblemDetails { Title = "There are not any instructors in stores !" });
            return Ok(instructors);
        }

        [HttpGet("{id:int}", Name = "GetInstructor")]
        public async Task<IActionResult> GetInstructor(int? id)
        {
            var instructor = await _db.Instructors.Include(i => i.ProductInstructors)
                .ThenInclude(p => p.Product).ThenInclude(p => p.Category)
                .Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Products = i.ProductInstructors.Select(p =>
                    new ProductDTO
                    {
                        Id = (int)p.ProductId,
                        Name = p.Product.Name,
                        Description = p.Product.Description,
                        Price = p.Product.Price,
                        ImageUrl = p.Product.ImageUrl,
                        CategoryId = p.Product.CategoryId,
                        CategoryName = p.Product.Category.Name,
                    }).ToList(),
                }).FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null) return BadRequest(new ProblemDetails { Title = "There is no instructor in stores !" });
            return Ok(instructor);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInstructor([FromForm] InstructorCreateDTO instructorCreate)
        {
            var newInstructor = new Instructor()
            {
                Name = instructorCreate.Name,
            };

            _db.Instructors.Add(newInstructor);
            var result = await _db.SaveChangesAsync() > 0;
            if (result) return CreatedAtRoute("GetInstructor", new { Id = newInstructor.Id }, newInstructor);
            return BadRequest(new ProblemDetails { Title = "Problem creating new instructor !" });
        }

        [HttpPost("toggleInsInPro")]
        public async Task<ActionResult> AddInstructorToProduct(int? productId, int? instructorId)
        {
            if (instructorId == 0 || instructorId == null || productId == 0 || productId == null) return BadRequest();
            var instructor = await _db.Instructors.FirstOrDefaultAsync(x => x.Id == instructorId);
            var product = await _db.Products.Include(p => p.ProductInstructors).FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null || instructor == null) return BadRequest();

            ProductInstructor instructorFromProduct = product.ProductInstructors.FirstOrDefault(x => x.InstructorId == instructorId);

            if (instructorFromProduct != null)
            {
                _db.ProductInstructors.Remove(instructorFromProduct);
            }
            else
            {
                _db.ProductInstructors.Add(new ProductInstructor { ProductId = productId, InstructorId = instructorId });
            }

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem adding product to instructor !" });
        }


        [HttpPut]
        public async Task<ActionResult> UpdateInstructor([FromForm] InstructorUpdateDTO instructorUpdate)
        {
            var instructorFromDb = await _db.Instructors.FirstOrDefaultAsync(i => i.Id == instructorUpdate.Id);
            if (instructorFromDb == null) return NotFound(new ProblemDetails { Title = "Instructor is not found !" });
            instructorFromDb.Name = instructorUpdate.Name;
            _db.Instructors.Update(instructorFromDb);
            var result = await _db.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem updating instructor !" });
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteInstructor(int? id)
        {
            var instructorFromDb = await _db.Instructors.Include(i => i.ProductInstructors).FirstOrDefaultAsync(i => i.Id == id);
            var instructorsFromProduct = await _db.ProductInstructors.Where(p => p.InstructorId == id).ToListAsync();

            if (instructorFromDb == null || instructorsFromProduct == null) return NotFound(new ProblemDetails { Title = "Instructor is not found !" });
            
            _db.Instructors.Remove(instructorFromDb);
            _db.ProductInstructors.RemoveRange(instructorsFromProduct);

            var result = await _db.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting instructor !" });
        }
    }
}
