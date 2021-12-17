using Microsoft.EntityFrameworkCore;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using BankYouBankruptDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankYouBankruptDatabaseImplement.Implements
{
    public class UserStorage : IUserStorage
    {
        public User CreateModel(UserBindingModels model, User user)
        {            
            user.FIO = model.FIO;           
            user.Email = model.Email;
            user.Password = model.Password;
            user.Number = model.Number;
            return user;
        }

        public UserViewModel GetElement(UserBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var user = context.Users
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.Email == model.Email);
                return user != null ?
                new UserViewModel
                {
                    Id = user.Id,
                    FIO = user.FIO,                    
                    Email = user.Email,
                    Password = user.Password,
                    Number = user.Number
                } : null;
            }
        }

        public List<UserViewModel> GetFilteredList(UserBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BankYouBankruptDatabase())
            {
                return context.Users
                .Where(rec => rec.Email.Equals(model.Email) && rec.Password.Equals(model.Password))
                .Select(rec => new UserViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,                    
                    Email = rec.Email,
                    Password = rec.Password,
                    Number = rec.Number,
                }).ToList();
            }
        }

        public List<UserViewModel> GetFullList()
        {
            using (var context = new BankYouBankruptDatabase())
            {
                return context.Users
                .Select(rec => new UserViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,                    
                    Email = rec.Email,
                    Password = rec.Password,
                    Number = rec.Number,
                }).ToList();
            }
        }

        public void Insert(UserBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                context.Users.Add(CreateModel(model, new User()));
                context.SaveChanges();
            }
        }

        public void Update(UserBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var user = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                if (user == null)
                {
                    throw new Exception("Пользователь не найдена");
                }
                CreateModel(model, user);
                context.SaveChanges();
            }
        }
        public void Delete(UserBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var user = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Пользователь не найдена");
                }
            }
        }
    }
}
