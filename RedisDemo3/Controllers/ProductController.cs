using Microsoft.AspNetCore.Mvc;
using RedisDemo3.Cache;
using RedisDemo3.DBContext;
using RedisDemo3.Entity;

namespace RedisDemo3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DbContextClass _context;
        private readonly ICacheService _cacheService;

        public ProductController(DbContextClass context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if(cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTime.Now.AddMinutes(5);
            cacheData = _context.Products.ToList();
            _cacheService.SetData<IEnumerable<Product>>("product", cacheData, expirationTime);
            return cacheData;
        }

        [HttpPost]
        public async Task<Product> SaveProduct(Product product)
        {
            var obj = await _context.Products.AddAsync(product);
            _cacheService.RemoveData("product");
            _context.SaveChanges();
            return obj.Entity;
        }

        [HttpPut]
        public void Put(Product product)
        {
            _context.Products.Update(product);
            _cacheService.RemoveData("product");
            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var filteredData = _context.Products.Find(id);
            if (filteredData is null)
            {
                return NotFound(); 
            }
            _context.Products.Remove(filteredData);
            _cacheService.RemoveData("product");
            _context.SaveChanges(); 
            return NoContent(); 
        }
    }
}
