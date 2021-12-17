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
    public class ApplicationsStorage : IApplicationStorage
    {
        public Applications CreateModel(ApplicationsBindingModels model, Applications application, BankYouBankruptDatabase context)
        {            
            application.AplicationSum = model.AplicationSum;
            application.AplicationDate = model.AplicationDate;
            application.AplicationNumber = model.AplicationNumber;            
            application.UserId = (int)model.UserId;           
            if (application.Id == 0)
            {
                context.Application.Add(application);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                List<MoneyTransferApplication> moneyTransferApplications = context.MoneyTransferApplications.Where(rec => rec.AplicationId == model.Id.Value).ToList();
                context.MoneyTransferApplications.RemoveRange(moneyTransferApplications.Where(rec => !model.ApplicationMoneyTransfer.ContainsKey(rec.MoneyTransferId)).ToList());
                context.SaveChanges();

                foreach (var moneyTransferApplication in moneyTransferApplications)
                {
                    if (model.ApplicationMoneyTransfer.ContainsKey(moneyTransferApplication.MoneyTransferId))
                    {
                        model.ApplicationMoneyTransfer.Remove(moneyTransferApplication.MoneyTransferId);
                    }
                }
            }
            context.SaveChanges();            
            foreach (KeyValuePair<int, (string, string)> CSP in model.ApplicationMoneyTransfer)
            {
                context.MoneyTransferApplications.Add(new MoneyTransferApplication
                {
                    AplicationId = application.Id,
                    MoneyTransferId = CSP.Key
                });
                context.SaveChanges();
            }
            return application;
        }

        public ApplicationsViewModel GetElement(ApplicationsBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var application = context.Application
                    .Include(rec => rec.AplicationMoneyTransfer)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.AplicationNumber == model.AplicationNumber);
                return application != null ?
                new ApplicationsViewModel
                {
                    Id = application.Id,
                    AplicationSum = application.AplicationSum,
                    AplicationDate = application.AplicationDate,
                    AplicationNumber = application.AplicationNumber,
                    UserId = application.UserId,
                    UserFIO = application.User.FIO,
                    ApplicationMoneyTransfer = application.AplicationMoneyTransfer.ToDictionary(rec => rec.MoneyTransferId, rec => (rec.MoneyTransfer.Sender, rec.MoneyTransfer.Recipient))
                } :
                null;
            }
        }

        public List<ApplicationsViewModel> GetFilteredList(ApplicationsBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.Application
                    .Include(rec => rec.AplicationMoneyTransfer)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.User)
                    .Where(rec => rec.AplicationNumber.Equals(model.AplicationNumber) || (model.UserId.HasValue && rec.UserId == model.UserId))
                    .ToList()
                    .Select(rec => new ApplicationsViewModel
                    {
                        Id = rec.Id,
                        AplicationSum = rec.AplicationSum,
                        AplicationDate = rec.AplicationDate,
                        AplicationNumber = rec.AplicationNumber,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        ApplicationMoneyTransfer = rec.AplicationMoneyTransfer.ToDictionary(recCSP => recCSP.MoneyTransferId, recCSP => (recCSP.MoneyTransfer.Sender, recCSP.MoneyTransfer.Recipient))
                    }).ToList();
            }
        }

        public List<ApplicationsViewModel> GetFullList()
        {
            using (var context = new BankYouBankruptDatabase())
            {
                return context.Application
                    .Include(rec => rec.AplicationMoneyTransfer)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.User)
                    .ToList()
                    .Select(rec => new ApplicationsViewModel
                    {
                        Id = rec.Id,
                        AplicationSum = rec.AplicationSum,
                        AplicationDate = rec.AplicationDate,
                        AplicationNumber = rec.AplicationNumber,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        ApplicationMoneyTransfer = rec.AplicationMoneyTransfer.ToDictionary(recCSP => recCSP.MoneyTransferId, recCSP => (recCSP.MoneyTransfer.Sender, recCSP.MoneyTransfer.Recipient))
                    }).ToList();
            }
        }

        public void Insert(ApplicationsBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Applications(), context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(ApplicationsBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Application.FirstOrDefault(rec => rec.Id == model.Id || rec.AplicationNumber == model.AplicationNumber);
                        if (element == null)
                        {
                            throw new Exception("Заявка не найдена");
                        }
                        CreateModel(model, element, context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(ApplicationsBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                var element = context.Application.FirstOrDefault(rec => rec.Id == model.Id || rec.AplicationNumber == model.AplicationNumber);
                if (element != null)
                {
                    context.Application.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Заявка не найдена");
                }
            }
        }
    }
}
