using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.HelperModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class ReportLogicExecutor
    {
        private readonly IReportStorage reportStorage;
        public ReportLogicExecutor(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }
        private Dictionary<int, ReportOperationsViewModel> GetOperationCard(List<CardsViewModel> selectedCards)
        {
            var report = reportStorage.GetOperationCard(selectedCards);
            return report;
        }

        public List<ReportApplicationCardsViewModel> GetNumberCardsActoins(ReportBindingModel model)
        {
            var report = reportStorage.GetNumberCardsActoins(model);
            return report.OrderBy(rec => rec.NumberCard).ThenBy(rec => rec.DatePassed).ToList();
        }

        public void SaveOperationCardToWordFile(ReportBindingModel model)
        {
            SaveToWordExecutor.CreateDoc(new ListOperationInfoExecutor
            {
                FileName = model.FileName,
                Title = "Список операций по указанным картам",
                Cards = GetOperationCard(model.Cards)
            });            
        }

        public void SaveOperationCardToExcelFile(ReportBindingModel model)
        {
            SaveToExcelExecution.CreateDoc(new ListOperationInfoExecutor
            {
                FileName = model.FileName,
                Title = "Список операций по указанным картам",
                Cards = GetOperationCard(model.Cards)
            });
        }

        public void SaveNumberCardsActionsToPdf(ReportBindingModel model)
        {
            SaveToPdfExecutor.CreateDoc(new PdfInfoExecutor
            {
                FileName = model.FileName,
                Title = "Список действий по картам",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Cards = GetNumberCardsActoins(model)
            });
        }
    }
}

