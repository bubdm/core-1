﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Domain.WebModel
{
    public class RegisterUserWebModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        [Remote("IsNameFree","Account")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтверждение пароля")]
        [DataType((DataType.Password))]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
