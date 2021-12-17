using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface IUserStorage
    {
        List<UserViewModel> GetFullList();
        List<UserViewModel> GetFilteredList(UserBindingModels model);
        UserViewModel GetElement(UserBindingModels model);
        void Insert(UserBindingModels model);
        void Update(UserBindingModels model);
        void Delete(UserBindingModels model);
    }
}
