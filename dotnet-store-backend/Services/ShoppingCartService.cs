using dotnet_store_backend.Models;

namespace dotnet_store_backend.Services;

// Services/ShoppingCartService.cs
public class ShoppingCartService
{
    private readonly Dictionary<string, ShoppingCart> _carts = new();

    public ShoppingCart GetCart(string cartId)
    {
        if (!_carts.ContainsKey(cartId))
            _carts[cartId] = new ShoppingCart { CartId = cartId };

        return _carts[cartId];
    }

    public void AddItem(string cartId, int productId, int quantity)
    {
        var cart = GetCart(cartId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            cart.Items.Add(new ShoppingCartItem { ProductId = productId, Quantity = quantity });
        else
            item.Quantity += quantity;
    }

    public void RemoveItem(string cartId, int productId)
    {
        var cart = GetCart(cartId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
            cart.Items.Remove(item);
    }
}
