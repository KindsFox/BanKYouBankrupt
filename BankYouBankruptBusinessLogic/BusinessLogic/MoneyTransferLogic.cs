using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class MoneyTransferLogic
    {
        private readonly IMoneyTransferStorage _moneyTransferStorage;
        public MoneyTransferLogic(IMoneyTransferStorage moneyTransferStorage)
        {
            _moneyTransferStorage = moneyTransferStorage;
        }
        public List<MoneyTransferViewModel> Read(MoneyTransferBindingModels model)
        {
            if (model == null)
            {
                return _moneyTransferStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<MoneyTransferViewModel> { _moneyTransferStorage.GetElement(model) };
            }
            return _moneyTransferStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(MoneyTransferBindingModels model)
        {
            MoneyTransferViewModel moneyTransfer = _moneyTransferStorage.GetElement(new MoneyTransferBindingModels
            {
                Id = model.Id
            });
            if (moneyTransfer != null && moneyTransfer.Id != model.Id)
            {
                throw new Exception("Карта уже существует");
            }
            if (model.Id.HasValue)
            {
                _moneyTransferStorage.Update(model);
            }
            else
            {
                _moneyTransferStorage.Insert(model);
            }
        }
        public void Delete(MoneyTransferBindingModels model)
        {
            MoneyTransferViewModel moneyTransfer = _moneyTransferStorage.GetElement(new MoneyTransferBindingModels
            {
                Id = model.Id
            });
            if (moneyTransfer == null)
            {
                throw new Exception("Карта не найдена");
            }
            _moneyTransferStorage.Delete(model);
        }
    }
}

