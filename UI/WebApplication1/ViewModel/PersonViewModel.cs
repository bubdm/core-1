using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.ViewModel
{
    public class PersonViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Нужно ввести фамилию")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длинна должна быть от 2 до 200")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Строка имела неверный формат (Либо русские, либо латинские символы начиная с заглавной буквы)")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Нужно ввести имя")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длинна должна быть от 2 до 200")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Строка имела неверный формат (Либо русские, либо латинские символы начиная с заглавной буквы)")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        [StringLength(200, ErrorMessage = "Длинна должна быть до 200")]
        public string Patronymic { get; set; }
        [Display(Name = "Возраст")]
        [Range(18, 100, ErrorMessage = "Возраст должен быть от 18 до 100")]
        public int Age { get; set; }
        [Display(Name = "День рождения")]
        public DateTime Birthday { get; set; }
        [Display(Name = "Количество детей, штук")]
        public int CountChildren { get; set; }
    }
}
