using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class UserLogic
    {
        private readonly IUserStorage _userStorage;
        public UserLogic(IUserStorage userStorage)
        {
            _userStorage = userStorage;
        }
        public List<UserViewModel> Read(UserBindingModels model)
        {
            if (model == null)
            {
                return _userStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<UserViewModel> { _userStorage.GetElement(model) };
            }
            return _userStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(UserBindingModels model)
        {
            UserViewModel user = _userStorage.GetElement(new UserBindingModels
            {
                Email = model.Email
            });
            if (user != null && user.Id != model.Id)
            {
                throw new Exception("Пользователь уже существует");
            }
            if (model.Id.HasValue)
            {
                _userStorage.Update(model);
            }
            else
            {
                _userStorage.Insert(model);
            }
        }

        public void Delete(UserBindingModels model)
        {
            UserViewModel user = _userStorage.GetElement(new UserBindingModels
            {
                Id = model.Id
            });
            if (user == null)
            {
                throw new Exception("Пользователь не найдена");
            }
            _userStorage.Delete(model);
        }
       
    }
}
