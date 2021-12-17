using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class OperationsLogic
    {
        private readonly IOperationsStorage _operationsStorage;
        public OperationsLogic(IOperationsStorage operationsStorage)
        {
            _operationsStorage = operationsStorage;
        }
        public List<OperationsViewModel> Read(OperationsBimdingModels model)
        {
            if (model == null)
            {
                return _operationsStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<OperationsViewModel> { _operationsStorage.GetElement(model) };
            }
            return _operationsStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(OperationsBimdingModels model)
        {
            OperationsViewModel operations = _operationsStorage.GetElement(new OperationsBimdingModels
            {
                OperationNumber = model.OperationNumber
            });
            if (operations != null && operations.Id != model.Id)
            {
                throw new Exception("Операция уже существует");
            }
            if (model.Id.HasValue)
            {
                _operationsStorage.Update(model);
            }
            else
            {
                _operationsStorage.Insert(model);
            }
        }
        public void Delete(OperationsBimdingModels model)
        {
            OperationsViewModel operations = _operationsStorage.GetElement(new OperationsBimdingModels
            {
                Id = model.Id
            });
            if (operations == null)
            {
                throw new Exception("Операция не найдена");
            }
            _operationsStorage.Delete(model);
        }
    }
}
