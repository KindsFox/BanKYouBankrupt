using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface IMoneyTransferStorage
    {
        List<MoneyTransferViewModel> GetFullList();
        List<MoneyTransferViewModel> GetFilteredList(MoneyTransferBindingModels model);
        MoneyTransferViewModel GetElement(MoneyTransferBindingModels model);
        void Insert(MoneyTransferBindingModels model);
        void Update(MoneyTransferBindingModels model);
        void Delete(MoneyTransferBindingModels model);
    }
}
