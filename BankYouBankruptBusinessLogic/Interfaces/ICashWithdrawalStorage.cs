using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface ICashWithdrawalStorage
    {
        List<CashWithdrawalViewModel> GetFullList();       
        CashWithdrawalViewModel GetElement(CashWithdrawalBindingModels model);
        void Insert(CashWithdrawalBindingModels model);
        void Update(CashWithdrawalBindingModels model);
        void Delete(CashWithdrawalBindingModels model);
    }
}
