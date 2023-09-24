using Fit_API.DataAccess.Data;
using Fit_API.DataAccess.Services;
using Fit_API.Models;
using Fit_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {
        private readonly ApplicationDbContext _db;
        private readonly ImageService _imageService;
        public ProductController(ApplicationDbContext db, ImageService imageService)
        {
            _db = db;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _db.Products.Include(p => p.Category)
                .Include(p => p.ProductInstructors).ThenInclude(c => c.Instructor)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Instructors = p.ProductInstructors.Select(p => new Instructor { Id = (int)p.InstructorId, Name = p.Instructor.Name}).ToList(),
                })
                .ToListAsync();
            if(products == null) return BadRequest(new ProblemDetails { Title = "There are not any products in stores !" });
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<IActionResult> GetAllProduct(int id)
        {
            var product = await _db.Products.Include(p => p.Category)
                .Include(p => p.ProductInstructors).ThenInclude(c => c.Instructor)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Instructors = p.ProductInstructors.Select(p => new Instructor { Id = (int)p.InstructorId, Name = p.Instructor.Name }).ToList(),
                })
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
            if (product == null) return BadRequest(new ProblemDetails { Title = "There is no product in stores !" });
            return Ok(product);
        }

        [HttpPost("createProduct")]
        public async Task<ActionResult> CreateProduct([FromForm] ProductCreateDTO productCreate)
        {

            var product = new Product()
            {
                Name = productCreate.Name,
                Price = productCreate.Price,
                Description = productCreate.Description,
                CategoryId = productCreate.CategoryId,
            };

            if (productCreate.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productCreate.File);

                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                product.ImageUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            _db.Products.Add(product);

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        }

        [HttpPost("toggleInsInPro")]
        public async Task<ActionResult> AddInstructorToProduct(int? productId, int? instructorId)
        {
            if (instructorId == 0 || instructorId == null || productId == 0 || productId == null) return BadRequest();
            var instructor = await _db.Instructors.FirstOrDefaultAsync(x => x.Id == instructorId);
            var product = await _db.Products.Include(p => p.ProductInstructors).FirstOrDefaultAsync(x => x.Id == productId);

            ProductInstructor instructorFromProduct = product.ProductInstructors.FirstOrDefault(x => x.InstructorId == instructorId);

            if (instructorFromProduct != null)
            {
                _db.ProductInstructors.Remove(instructorFromProduct);
            } else
            {
                _db.ProductInstructors.Add(new ProductInstructor { ProductId = productId, InstructorId = instructorId });
            }

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem adding instructor to product !" });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromForm] ProductUpdateDTO productUpdate)
        {
            var productFromDb = await _db.Products.FirstOrDefaultAsync(x => x.Id == productUpdate.Id);
            if (productFromDb == null) return NotFound();

            productFromDb.Name = productUpdate.Name;
            productFromDb.Description = productUpdate.Description;
            productFromDb.Price = productUpdate.Price;
            productFromDb.CategoryId = productUpdate.CategoryId;

            if (productUpdate.File != null)
            {
                var imageUploadResult = await _imageService.AddImageAsync(productUpdate.File);

                if (imageUploadResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageUploadResult.Error.Message });

                if (!string.IsNullOrEmpty(productFromDb.PublicId))
                    await _imageService.DeleteImageAsync(productFromDb.PublicId);

                productFromDb.ImageUrl = imageUploadResult.SecureUrl.ToString();
                productFromDb.PublicId = imageUploadResult.PublicId;
            }

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return Ok(productFromDb);

            return BadRequest(new ProblemDetails { Title = "Problem Updating product !" });
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int? id)
        {
            var productFromDb = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            var productsContainsIns = await _db.ProductInstructors.Where(p => p.ProductId == id).ToListAsync();

            if (productFromDb == null || productsContainsIns == null) return NotFound(new ProblemDetails { Title = "Product is not found !"});

            _db.Products.Remove(productFromDb);
            _db.ProductInstructors.RemoveRange(productsContainsIns);

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem Deleteing product !" });
        }
    }
}
