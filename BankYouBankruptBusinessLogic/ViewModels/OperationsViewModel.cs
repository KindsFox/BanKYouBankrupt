using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class OperationsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Дата операции")]
        public DateTime OperationDate { get; set; }
        [DisplayName("Номер операции")]
        public int OperationNumber { get; set; }
        [DisplayName("Тип операции")]
        public string OperationType { get; set; }
        public int UserId { get; set; }
        [DisplayName("ФИО пользователя")]
        public string UserFIO { get; set; }
    }
}
