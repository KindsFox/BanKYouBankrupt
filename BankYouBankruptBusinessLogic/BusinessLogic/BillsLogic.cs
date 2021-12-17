using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class BillsLogic
    {
        private readonly IBillsStorage _billsStorage;
        public BillsLogic(IBillsStorage billsStorage)
        {
            _billsStorage = billsStorage;
        }
        public List<BillsViewModel> Read(BillsBindingModels model)
        {
            if (model == null)
            {
                return _billsStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<BillsViewModel> { _billsStorage.GetElement(model) };
            }
            return _billsStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(BillsBindingModels model)
        {
            BillsViewModel bills = _billsStorage.GetElement(new BillsBindingModels
            {
                BillsNumber = model.BillsNumber
            });
            if (bills != null && bills.Id != model.Id)
            {
                throw new Exception("Счет уже существует");
            }
            if (model.Id.HasValue)
            {
                _billsStorage.Update(model);
            }
            else
            {
                _billsStorage.Insert(model);
            }
        }
        public void Delete(BillsBindingModels model)
        {
            BillsViewModel application = _billsStorage.GetElement(new BillsBindingModels
            {
                Id = model.Id
            });
            if (application == null)
            {
                throw new Exception("Счет не найден");
            }
            _billsStorage.Delete(model);
        }
    }
}
