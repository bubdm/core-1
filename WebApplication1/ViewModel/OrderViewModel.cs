using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Нужно ввести название заказа")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Нужно обязательно ввести номер телефона")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Нужно обязательно ввести свой адрес")]
        public string Address { get; set; }
    }
}
