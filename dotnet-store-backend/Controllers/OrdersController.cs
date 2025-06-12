using dotnet_store_backend.Data;
using dotnet_store_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly StoreDbContext _context;

    public OrdersController(StoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .ToList();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();
        return Ok(order);
    }

    // DTO for creating order
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var customer = await _context.Customers.FindAsync(dto.CustomerId);
        if (customer == null) return BadRequest("Invalid CustomerId");

        if (dto.Items == null || !dto.Items.Any()) return BadRequest("Order must have at least one item.");

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var itemDto in dto.Items)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId);
            if (product == null) return BadRequest($"Invalid ProductId: {itemDto.ProductId}");

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                Price = product.Price * itemDto.Quantity
            };
            totalAmount += orderItem.Price;
            orderItems.Add(orderItem);
        }

        var order = new Order
        {
            CustomerId = customer.Id,
            OrderDate = DateTime.UtcNow,
            Items = orderItems,
            TotalAmount = totalAmount
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        // Remove related order items first if necessary
        _context.OrderItems.RemoveRange(order.Items);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, CreateOrderDto dto)
    {
        var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        var customer = await _context.Customers.FindAsync(dto.CustomerId);
        if (customer == null) return BadRequest("Invalid CustomerId");

        if (dto.Items == null || !dto.Items.Any()) return BadRequest("Order must have at least one item.");

        // Remove old items
        _context.OrderItems.RemoveRange(order.Items);

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var itemDto in dto.Items)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId);
            if (product == null) return BadRequest($"Invalid ProductId: {itemDto.ProductId}");

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                Price = product.Price * itemDto.Quantity
            };
            totalAmount += orderItem.Price;
            orderItems.Add(orderItem);
        }

        order.CustomerId = customer.Id;
        order.OrderDate = DateTime.UtcNow;
        order.Items = orderItems;
        order.TotalAmount = totalAmount;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
