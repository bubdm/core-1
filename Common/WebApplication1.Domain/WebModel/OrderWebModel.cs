using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Domain.WebModel
{
    /// <summary> Заказ </summary>
    public class OrderWebModel
    {
        /// <summary> Название заказа </summary>
        [Required(ErrorMessage = "Нужно ввести название заказа")]
        public string Name { get; set; }
        /// <summary> Телефон </summary>
        [Required(ErrorMessage = "Нужно обязательно ввести номер телефона")]
        public string Phone { get; set; }
        /// <summary> Адрес </summary>
        [Required(ErrorMessage = "Нужно обязательно ввести свой адрес")]
        public string Address { get; set; }
    }
}
