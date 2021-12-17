using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface IBillsStorage
    {
        List<BillsViewModel> GetFullList();
        List<BillsViewModel> GetFilteredList(BillsBindingModels model);
        BillsViewModel GetElement(BillsBindingModels model);
        void Insert(BillsBindingModels model);
        void Update(BillsBindingModels model);
        void Delete(BillsBindingModels model);
    }
}
