
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
    public class CashWithdrawalStorage : ICashWithdrawalStorage
    {
        public CashWithdrawal CreateModel(CashWithdrawalBindingModels model, CashWithdrawal cashWithdrawal)
        {
            cashWithdrawal.ApplicationId = model.AplicationId;
            cashWithdrawal.AvailabilityApplication = model.AvailabilityApplication;           
            return cashWithdrawal;
        }
        public CashWithdrawalViewModel GetElement(CashWithdrawalBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var serviceRecording = context.CashWithdrawals.Include(rec => rec.Application)
                    .Include(rec => rec.Application)                    
                    .FirstOrDefault(rec => rec.ApplicationId == model.AplicationId);
                return serviceRecording != null ?
                new CashWithdrawalViewModel
                {
                    AplicationsId = serviceRecording.ApplicationId,
                    AvailabilityApplication = serviceRecording.AvailabilityApplication,
                    ApplicationNumber = serviceRecording.Application.AplicationNumber
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
            using (var context = new BankYouBankruptDatabase())
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

        public List<CashWithdrawalViewModel> GetFullList()
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.CashWithdrawals.Include(rec => rec.Application)
                    .Include(rec => rec.Application)                    
                    .Select(rec => new CashWithdrawalViewModel
                    {
                        AplicationsId = rec.ApplicationId,
                        AvailabilityApplication = rec.AvailabilityApplication,
                        ApplicationNumber = rec.Application.AplicationNumber,                        
                    }).ToList();
            }
        }

        public void Insert(CashWithdrawalBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                context.CashWithdrawals.Add(CreateModel(model, new CashWithdrawal()));
                context.SaveChanges();
            }
        }

        public void Update(CashWithdrawalBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var serviceRecording = context.CashWithdrawals.FirstOrDefault(rec => rec.ApplicationId == model.AplicationId);
                if (serviceRecording == null)
                {
                    throw new Exception("Выдача наличных не найдена");
                }
                CreateModel(model, serviceRecording);
                context.SaveChanges();
            }
        }
        public void Delete(CashWithdrawalBindingModels model)
        { 
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var serviceRecording = context.CashWithdrawals.FirstOrDefault(rec => rec.ApplicationId == model.AplicationId);
                if (serviceRecording != null)
                {
                    context.CashWithdrawals.Remove(serviceRecording);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Выдача наличных не найдена");
                }
            }
        }
    }
}
