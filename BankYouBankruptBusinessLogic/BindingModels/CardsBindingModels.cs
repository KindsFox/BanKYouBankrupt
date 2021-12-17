using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BindingModels
{
    public class CardsBindingModels
    {
        public int? Id { get; set; }
        //номер карты
        public string CardsNumder { get; set; }
        //код безопасности
        public int SecurityCode { get; set; }
        //дата окончания обслуживания
        public DateTime ServiceEndDate { get; set; }
        public int? UserId { get; set; }
        public int? ApplicationId { get; set; }
        public int? OperationId { get; set; }
        public Dictionary<int, decimal> CardsAplications { get; set; }
        public Dictionary<int, string> CardsOperations { get; set; }
        public List<int> SelectedCards
        {
            get; set;
        }
    }
}
