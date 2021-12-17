using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class CashWithdrawalLogic
    {
        private readonly ICashWithdrawalStorage _cashWithdrawalStorage;
        public CashWithdrawalLogic(ICashWithdrawalStorage cashWithdrawalStorage)
        {
            _cashWithdrawalStorage = cashWithdrawalStorage;
        }
        public List<CashWithdrawalViewModel> Read(CashWithdrawalBindingModels model)
        {
            if (model == null)
            {
                return _cashWithdrawalStorage.GetFullList();
            }            
            return new List<CashWithdrawalViewModel> { _cashWithdrawalStorage.GetElement(model) };
        }
        public void CreateOrUpdate(CashWithdrawalBindingModels model)
        {
            CashWithdrawalViewModel cashWithdrawals = _cashWithdrawalStorage.GetElement(new CashWithdrawalBindingModels
            {
               AplicationId = model.AplicationId
            });
            if (cashWithdrawals != null)
            {
                _cashWithdrawalStorage.Update(model);                
            }            
            else
            {
                _cashWithdrawalStorage.Insert(model);
            }
        }
        public void Delete(CashWithdrawalBindingModels model)
        {
            CashWithdrawalViewModel cashWithdrawals = _cashWithdrawalStorage.GetElement(new CashWithdrawalBindingModels
            {
                AplicationId = model.AplicationId
            });
            if (cashWithdrawals == null)
            {
                throw new Exception("Перевод денег не найден");
            }
            _cashWithdrawalStorage.Delete(model);
        }
    }
}

