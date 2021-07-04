namespace WebApplication1.Domain.DTO
{
    /// <summary> Пункт заказа </summary>
    public class OrderItemDTO
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Идентификатор товара </summary>
        public int ProductId { get; set; }
        /// <summary> Стоимость </summary>
        public decimal Price { get; set; }
        /// <summary> Количество </summary>
        public int Quantity { get; set; }
    }
}