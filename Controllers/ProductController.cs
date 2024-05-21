using Microsoft.AspNetCore.Mvc;
using dotnet_training.DataAccess;
using dotnet_training.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_training.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {

        private readonly ApplicationContext _context;
        public ProductController(ApplicationContext context) 
        {
            _context = context;
        }

        [HttpGet("Products")]
        [Authorize(Roles = "admin,user")]
        
        public IActionResult GetProducts() 
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("Product/{id}")]
        [Authorize(Roles = "admin,user")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        public IActionResult Save([FromBody] Product product) 
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("Update")]
        [Authorize(Roles = "admin")]
        public IActionResult Update([FromBody] Product product) 
        {
            var result = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id);

            if(result == null)
            {
                return NotFound();
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete([FromQuery] int id)
        {
            var deleteProduct = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if(deleteProduct == null)
            {
                return NotFound();
            }

            // _context.Products.Remove(deleteProduct); 

            _context.Products.Entry(deleteProduct).State = EntityState.Deleted;
            _context.SaveChanges();

            return Ok();
        }
    }
}