using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class CardsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Номер карты")]
        public string CardsNumber { get; set; }
        [DisplayName("Код безопастности карты")]
        public int SecurityCode { get; set; }
        [DisplayName("Дата окончания обслуживания")]
        public string ServiceEndDateString { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public int UserId { get; set; }
        [DisplayName("ФИО пользователя")]
        public string UserFIO { get; set; }
        public Dictionary<int, decimal> CardsAplications { get; set; }
        public Dictionary<int, string> CardsOperations { get; set; }
    }
}
