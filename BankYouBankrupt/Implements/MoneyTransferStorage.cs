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
    public class MoneyTransferStorage : IMoneyTransferStorage
    {

        public MoneyTransferViewModel GetElement(MoneyTransferBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                MoneyTransfer moneyTransfer = context.MoneyTransfers
                    .Include(rec => rec.Operation)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.Sender == model.Sender && model.Recipient == model.Recipient);
                return moneyTransfer != null ?
                new MoneyTransferViewModel
                {
                    Id = moneyTransfer.Id,
                    Sender = moneyTransfer.Sender,
                    Recipient = moneyTransfer.Recipient,
                    SendersCard = moneyTransfer.SendersCard,
                    RecipientsCard = moneyTransfer.RecipientsCard,
                    OperationId = moneyTransfer.Operation.Id,
                    OperationNumber = moneyTransfer.Operation.OperationNumber
                } : null;
            }
        }

        public List<MoneyTransferViewModel> GetFilteredList(MoneyTransferBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.MoneyTransfers
                    .Include(rec => rec.MoneyTransferBills)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.Operation)
                    .Where(rec => model.Sender == rec.Sender) 
                    .Select(rec => new MoneyTransferViewModel
                    {
                        Id = rec.Id,
                        Sender = rec.Sender,
                        Recipient = rec.Recipient,
                        SendersCard = rec.SendersCard,
                        RecipientsCard = rec.RecipientsCard,
                        OperationId = rec.Operation.Id,
                        OperationNumber = rec.Operation.OperationNumber
                    }).ToList();
            }
        }

        public List<MoneyTransferViewModel> GetFullList()
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.MoneyTransfers
                    .Include(rec => rec.MoneyTransferBills)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.Operation)
                    .Select(rec => new MoneyTransferViewModel
                    {
                        Id = rec.Id,
                        Sender = rec.Sender,
                        Recipient = rec.Recipient,
                        SendersCard = rec.SendersCard,
                        RecipientsCard = rec.RecipientsCard,
                        OperationId = rec.Operation.Id,
                        OperationNumber = rec.Operation.OperationNumber
                    }).ToList();
            }
        }

        public void Insert(MoneyTransferBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                context.MoneyTransfers.Add(CreateModel(model, new MoneyTransfer()));
                context.SaveChanges();
            }
        }

        public void Update(MoneyTransferBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                MoneyTransfer moneyTransfer = context.MoneyTransfers.FirstOrDefault(rec => rec.Id == model.Id);
                if (moneyTransfer == null)
                {
                    throw new Exception("Перевод денег не найден");
                }
                CreateModel(model, moneyTransfer);
                context.SaveChanges();
            }
        }

        public void Delete(MoneyTransferBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                MoneyTransfer moneyTransfer = context.MoneyTransfers.FirstOrDefault(rec => rec.Id == model.Id);
                if (moneyTransfer != null)
                {
                    context.MoneyTransfers.Remove(moneyTransfer);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Перевод денег не найден");
                }
            }
        }

        public MoneyTransfer CreateModel(MoneyTransferBindingModels model, MoneyTransfer moneyTransfer)
        {
            moneyTransfer.Sender = model.Sender;
            moneyTransfer.Recipient = model.Recipient;
            moneyTransfer.SendersCard = model.SendersCard;
            moneyTransfer.RecipientsCard = model.RecipientsCard;
            moneyTransfer.OperationId = model.OperationId;
            return moneyTransfer;
        }
    }
}
