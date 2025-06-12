namespace dotnet_store_backend.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
}