namespace WebApplication1.Domain.WebModel
{
    public class UserOrderWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
