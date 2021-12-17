using System.ComponentModel;
using System.Collections.Generic;
using System;


namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class ApplicationsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Сумма заявки")]
        public decimal AplicationSum { get; set; }
        [DisplayName("Дата заявки")]
        public DateTime AplicationDate { get; set; }
        [DisplayName("Номер заявки")]
        public int AplicationNumber { get; set; }
        public int UserId { get; set; }
        [DisplayName("ФИО пользователя")]
        public string UserFIO { get; set; }
        public Dictionary<int, (string, string)> ApplicationMoneyTransfer { get; set; }
    }
}
