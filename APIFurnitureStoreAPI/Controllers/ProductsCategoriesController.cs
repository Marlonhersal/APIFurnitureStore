using APIFurnitureStore.Data;
using APIFurnitureStore.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIFurnitureStoreAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsCategoriesController : ControllerBase
    {

        private readonly APIFurnitureStoreContext _context;
        public ProductsCategoriesController(APIFurnitureStoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = await _context.ProductCategories.ToListAsync();
            return Ok(lista);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var client = await _context.ProductCategories.FirstOrDefaultAsync((pc => pc.Id == id));
            if (client == null) return NotFound();
            return Ok(client);
        }
        [HttpPost]
        public async Task<IActionResult> Post(ProductCategory productcategory)
        {
            await _context.ProductCategories.AddAsync(productcategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Post", productcategory.Id, productcategory);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ProductCategory productcategory)
        {
            _context.ProductCategories.Update(productcategory);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(ProductCategory productcategory)
        {
            if (productcategory == null) return NotFound();
            _context.ProductCategories.Remove(productcategory);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
