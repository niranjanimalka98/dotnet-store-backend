namespace dotnet_store_backend.Models;

public class Customer
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }  // Store hashed password
    public ICollection<Order> Orders { get; set; }
}