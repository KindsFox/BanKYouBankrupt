using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BindingModels
{
    //заявки
    public class ApplicationsBindingModels
    {
        public int? Id { get; set; }
        public decimal AplicationSum { get; set; }
        public DateTime AplicationDate { get; set; }
        public int AplicationNumber { get; set; }        
        public int? UserId { get; set; }      
        public Dictionary<int, (string, string)> ApplicationMoneyTransfer { get; set; }
    }
}
