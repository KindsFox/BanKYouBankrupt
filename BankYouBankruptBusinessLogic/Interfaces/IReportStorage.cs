using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface IReportStorage
    {
        List<ReportApplicationCardsViewModel> GetNumberCardsActoins(ReportBindingModel model);
        Dictionary<int, ReportOperationsViewModel> GetOperationCard(List<CardsViewModel> selectedCards);
    }
}
