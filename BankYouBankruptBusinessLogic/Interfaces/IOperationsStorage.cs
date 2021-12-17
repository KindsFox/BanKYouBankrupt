using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface IOperationsStorage
    {
        List<OperationsViewModel> GetFullList();
        List<OperationsViewModel> GetFilteredList(OperationsBimdingModels model);
        OperationsViewModel GetElement(OperationsBimdingModels model);
        void Insert(OperationsBimdingModels model);
        void Update(OperationsBimdingModels model);
        void Delete(OperationsBimdingModels model);
    }
}
