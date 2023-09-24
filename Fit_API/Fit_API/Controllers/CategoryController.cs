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
    public class CategoryController : BaseAPIController
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _db.Categories.ToListAsync();

            if(categories == null) return NotFound(new ProblemDetails {  Title = "Categories is null !" });

            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _db.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound(new ProblemDetails { Title = "Category is not found !" } );
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromForm] CategoryCreateDTO model)
        {
            Category newCategory = new()
            {
                Name = model.Name,
            };

            _db.Categories.Add(newCategory);
            var result = await _db.SaveChangesAsync() > 0;
            if (result) return CreatedAtRoute("GetCategory", new { id = newCategory.Id }, newCategory);
            return BadRequest(new ProblemDetails { Title = "Problem Create Category !" });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCategory([FromForm] CategoryUpdateDTO model)
        {
            var categoryFromDb = await _db.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (categoryFromDb == null) return BadRequest(new ProblemDetails { Title = "Category is not existed !" });

            categoryFromDb.Name = model.Name;
            _db.Categories.Update(categoryFromDb);
            var result = await _db.SaveChangesAsync() > 0;
            if(result) return Ok(categoryFromDb);
            return BadRequest(new ProblemDetails { Title = "Problem Updating Category !" });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveCategory(int id)
        {
            var categoryFromDb = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoryFromDb == null) return BadRequest(new ProblemDetails { Title = "Category is not existed !" });
            _db.Categories.Remove(categoryFromDb);
            var result = await _db.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem Deleting Category !" });
        }
    }
}
