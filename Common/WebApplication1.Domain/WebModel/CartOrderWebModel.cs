namespace WebApplication1.Domain.WebModel
{
    public class CartOrderWebModel
    {
        public CartWebModel Cart { get; set; }
        public OrderWebModel Order { get; set; } = new();
    }
}
