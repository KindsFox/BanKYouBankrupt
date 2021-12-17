using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankYouBankruptDatabaseImplement.Implements
{
    public class ReportStorage : IReportStorage
    {
        private readonly ICardsStorage cardsStorage;
        private readonly IApplicationStorage applicationStorage;
        private readonly IOperationsStorage operationsStorage;
        public ReportStorage(ICardsStorage cardsStorage, IApplicationStorage applicationStorage, IOperationsStorage operationsStorage)
        {
            this.cardsStorage = cardsStorage;
            this.applicationStorage = applicationStorage;
            this.operationsStorage = operationsStorage;
        }
        public Dictionary<int, ReportOperationsViewModel> GetOperationCard(List<CardsViewModel> selectedCards)
        {
            var cards = cardsStorage.GetFilteredList(new CardsBindingModels
            {
                SelectedCards = selectedCards.Select(rec => rec.Id)
                .ToList()
            });
            var record = new Dictionary<int, ReportOperationsViewModel>();

            cards.ForEach(rec =>
            {
                var cardoperation = rec.CardsOperations.ToList();
                var operations = new Dictionary<int, (int, string, DateTime)>();
                cardoperation.ForEach(recO =>
                {
                    var operation = operationsStorage.GetElement(new OperationsBimdingModels
                    {
                        Id = recO.Key
                    });
                    if (operation != null && !operations.ContainsKey(operation.Id))
                    {
                        operations.Add(operation.Id, (operation.OperationNumber, operation.OperationType, operation.OperationDate));
                    }
                });
                record.Add(rec.Id, new ReportOperationsViewModel
                {
                    CardsNumder = rec.CardsNumber,
                    UserFIO = rec.UserFIO,
                    Operation = operations
                });
            });
            return record;
        }

        public List<ReportApplicationCardsViewModel> GetNumberCardsActoins(ReportBindingModel model)
        {
            var cards = cardsStorage.GetFilteredList(new CardsBindingModels
            {
                UserId = model.UserId
            });

            var listReportNumberCards = new List<ReportApplicationCardsViewModel>();
            cards.ForEach(card =>
            {
                card.CardsAplications.ToList().ForEach(app =>
                {
                    var application = applicationStorage.GetElement(new ApplicationsBindingModels { Id = app.Key });
                    if (model.DateFrom.HasValue && model.DateTo.HasValue && application.AplicationDate >= model.DateFrom && application.AplicationDate <= model.DateTo)
                    {
                        listReportNumberCards.Add(new ReportApplicationCardsViewModel
                        {
                            NumberCard = card.CardsNumber,
                            NumberAction = "Заявка: " + application.AplicationNumber,
                            Information = "Сумма: " + application.AplicationSum + " Р",
                            DatePassed = application.AplicationDate.ToLocalTime()
                        });
                    }
                });
                card.CardsOperations.ToList().ForEach(oper =>
                {
                    var operation = operationsStorage.GetElement(new OperationsBimdingModels { Id = oper.Key });
                    if (model.DateFrom.HasValue && model.DateTo.HasValue && operation.OperationDate >= model.DateFrom && operation.OperationDate <= model.DateTo)
                    {
                        listReportNumberCards.Add(new ReportApplicationCardsViewModel
                        {
                            NumberCard = card.CardsNumber,
                            NumberAction = "Операция: " + operation.OperationNumber,
                            Information = "Вид операции: " + operation.OperationType,
                            DatePassed = operation.OperationDate.ToLocalTime()
                        });
                    }
                });
            });
            return listReportNumberCards;
        }
    }
}
