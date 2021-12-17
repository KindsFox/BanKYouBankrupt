using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface IApplicationStorage
    {
        List<ApplicationsViewModel> GetFullList();
        List<ApplicationsViewModel> GetFilteredList(ApplicationsBindingModels model);
        ApplicationsViewModel GetElement(ApplicationsBindingModels model);
        void Insert(ApplicationsBindingModels model);
        void Update(ApplicationsBindingModels model);
        void Delete(ApplicationsBindingModels model);
    }
}
