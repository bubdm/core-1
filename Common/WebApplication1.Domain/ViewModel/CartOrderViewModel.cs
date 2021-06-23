namespace WebApplication1.Domain.ViewModel
{
    public class CartOrderViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; } = new();
    }
}
