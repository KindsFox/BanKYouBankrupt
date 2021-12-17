using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BindingModels
{
    public class ReportBindingModel
    {
        public string FileName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? UserId { get; set; }
        public List<CardsViewModel> Cards { get; set; }
    }
}
