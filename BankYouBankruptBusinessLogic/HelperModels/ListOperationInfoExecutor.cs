using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.HelperModels
{
    public class ListOperationInfoExecutor
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public Dictionary<int, ReportOperationsViewModel> Cards { get; set; }
    }
}
