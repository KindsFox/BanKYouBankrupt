using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class ApplicationLogic
    {
        private readonly IApplicationStorage _applicationStorage;
        public ApplicationLogic(IApplicationStorage applicationStorage)
        {
            _applicationStorage = applicationStorage;
        }
        public List<ApplicationsViewModel> Read(ApplicationsBindingModels model)
        {
            if (model == null)
            {
                return _applicationStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ApplicationsViewModel> { _applicationStorage.GetElement(model) };
            }
            return _applicationStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(ApplicationsBindingModels model)
        {
            ApplicationsViewModel application = _applicationStorage.GetElement(new ApplicationsBindingModels
            {
                AplicationNumber = model.AplicationNumber
            });
            if (application != null && application.Id != model.Id)
            {
                throw new Exception("Заявка уже существует");
            }
            if (model.Id.HasValue)
            {
                _applicationStorage.Update(model);
            }
            else
            {
                _applicationStorage.Insert(model);
            }
        }
        public void Delete(ApplicationsBindingModels model)
        {
            ApplicationsViewModel application = _applicationStorage.GetElement(new ApplicationsBindingModels
            {
                Id = model.Id
            });
            if (application == null)
            {
                throw new Exception("Заявка не найдена");
            }
            _applicationStorage.Delete(model);
        }
    }
}
