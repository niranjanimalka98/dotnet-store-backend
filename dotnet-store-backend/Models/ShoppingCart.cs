namespace dotnet_store_backend.Models;

public class ShoppingCart
{
    public string CartId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
}