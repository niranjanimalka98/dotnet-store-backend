using dotnet_store_backend.Data;
using dotnet_store_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreDbContext _context;

    public ProductsController(StoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _context.Products.Include(p => p.Category).ToList();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products.Include(p => p.Category)
                                             .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        // Optionally validate category exists
        var category = await _context.Categories.FindAsync(product.CategoryId);
        if (category == null) return BadRequest("Invalid CategoryId");

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
    {
        if (id != updatedProduct.Id) return BadRequest();

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var category = await _context.Categories.FindAsync(updatedProduct.CategoryId);
        if (category == null) return BadRequest("Invalid CategoryId");

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.ImageUrl = updatedProduct.ImageUrl;
        product.CategoryId = updatedProduct.CategoryId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
