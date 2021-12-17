using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [DisplayName("ФИО пользователя")]
        public string FIO { get; set; }       
        [DisplayName("Почта")]
        public string Email { get; set; }
        [DisplayName("Пароль")]
        public string Password { get; set; }
        [DisplayName("Номер телефона")]
        public string Number { get; set; }
        
    }
}
