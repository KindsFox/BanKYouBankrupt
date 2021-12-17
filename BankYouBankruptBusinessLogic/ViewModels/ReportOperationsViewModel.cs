using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class ReportOperationsViewModel
    {
        public string CardsNumder { get; set; }
        public string UserFIO { get; set; }
        public Dictionary<int, (int,string, DateTime)> Operation { get; set; }
    }
}
