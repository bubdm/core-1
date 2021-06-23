namespace WebApplication1.ViewModel
{
    public class CartOrderViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; } = new();
    }
}
