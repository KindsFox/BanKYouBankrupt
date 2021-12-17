using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using BankYouBankruptDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankYouBankruptDatabaseImplement.Implements
{
    public class BillsStorage : IBillsStorage
    {
        public Bills CreateModel(BillsBindingModels model, Bills bills, BankYouBankruptDatabase context)
        {
            bills.BillsNumber = model.BillsNumber;
            bills.BillsBalance = model.BillsBalance;
            if (bills.Id == 0)
            {
                context.Bill.Add(bills);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                List<MoneyTransferBill> moneyTransferBills = context.MoneyTransferBills.Where(rec => rec.BillsId == model.Id.Value).ToList();
                context.MoneyTransferBills.RemoveRange(moneyTransferBills.Where(rec => !model.BillsMoneyTransfer.ContainsKey(rec.MoneyTransferId)).ToList());
                context.SaveChanges();

                foreach (var moneyTransferApplication in moneyTransferBills)
                {
                    if (model.BillsMoneyTransfer.ContainsKey(moneyTransferApplication.MoneyTransferId))
                    {
                        model.BillsMoneyTransfer.Remove(moneyTransferApplication.MoneyTransferId);
                    }
                }

                List<BillCashWithdrawal> billCashWithdrawals = context.CashWithdrawalBills.Where(rec => rec.BillsId == model.Id.Value).ToList();
                context.CashWithdrawalBills.RemoveRange(billCashWithdrawals.Where(rec => !model.BillCashWithdrawalId.ContainsKey(rec.CashWithdrawalId)).ToList());
                context.SaveChanges();

                foreach (var billCash in billCashWithdrawals)
                {
                    if (model.BillsMoneyTransfer.ContainsKey(billCash.CashWithdrawalId))
                    {
                        model.BillsMoneyTransfer.Remove(billCash.CashWithdrawalId);
                    }
                }
            }
            context.SaveChanges();
            foreach (KeyValuePair<int, string> CSP in model.BillsMoneyTransfer)
            {
                context.MoneyTransferBills.Add(new MoneyTransferBill
                {
                    BillsId = bills.Id,
                    MoneyTransferId = CSP.Key
                });
                context.SaveChanges();
            }

            context.SaveChanges();
            foreach (KeyValuePair<int, bool> CSP in model.BillCashWithdrawalId)
            {
                context.CashWithdrawalBills.Add(new BillCashWithdrawal
                {
                    BillsId = bills.Id,
                    CashWithdrawalId = CSP.Key
                });
                context.SaveChanges();
            }
            return bills;
        }

        public BillsViewModel GetElement(BillsBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var application = context.Bill
                    .Include(rec => rec.MoneyTransferBills)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.CashWithdrawalBills)
                    .ThenInclude(rec => rec.CashWithdrawals)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.BillsNumber == model.BillsNumber);
                return application != null ?
                new BillsViewModel
                {
                    Id = application.Id,
                    BillsNumber = application.BillsNumber,
                    BillsBalance = application.BillsBalance,
                    BillCashWithdrawalId = application.CashWithdrawalBills.ToDictionary(rec => rec.CashWithdrawalId, rec => rec.CashWithdrawals.AvailabilityApplication),
                    MoneyTransferId = application.MoneyTransferBills.ToDictionary(rec => rec.MoneyTransferId, rec => rec.MoneyTransfer.Sender)
                } :
                null;
            }
        }

        public List<BillsViewModel> GetFilteredList(BillsBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BankYouBankruptDatabase())
            {
                return context.Bill
                    .Include(rec => rec.MoneyTransferBills)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.CashWithdrawalBills)
                    .ThenInclude(rec => rec.CashWithdrawals)
                    .Where(rec => rec.BillsNumber.Equals(model.BillsNumber))
                    .ToList()
                    .Select(rec => new BillsViewModel
                    {
                        Id = rec.Id,
                        BillsNumber = rec.BillsNumber,
                        BillsBalance = rec.BillsBalance,
                        BillCashWithdrawalId = rec.CashWithdrawalBills.ToDictionary(recCSR => recCSR.CashWithdrawalId, recCSR => recCSR.CashWithdrawals.AvailabilityApplication),
                        MoneyTransferId = rec.MoneyTransferBills.ToDictionary(recCSR => recCSR.MoneyTransferId, recCSR => recCSR.MoneyTransfer.Sender)
                    }).ToList();
            }
        }

        public List<BillsViewModel> GetFullList()
        {
            using (var context = new BankYouBankruptDatabase())
            {
                return context.Bill
                    .Include(rec => rec.MoneyTransferBills)
                    .ThenInclude(rec => rec.MoneyTransfer)
                    .Include(rec => rec.CashWithdrawalBills)
                    .ThenInclude(rec => rec.CashWithdrawals)
                    .ToList()
                    .Select(rec => new BillsViewModel
                    {
                        Id = rec.Id,
                        BillsNumber = rec.BillsNumber,
                        BillsBalance = rec.BillsBalance,
                        BillCashWithdrawalId = rec.CashWithdrawalBills.ToDictionary(recCSR => recCSR.CashWithdrawalId, recCSR => recCSR.CashWithdrawals.AvailabilityApplication),
                        MoneyTransferId = rec.MoneyTransferBills.ToDictionary(recCSR => recCSR.MoneyTransferId, recCSR => recCSR.MoneyTransfer.Sender)
                    }).ToList();
            }
        }

        public void Insert(BillsBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Bills(), context);
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

        public void Update(BillsBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Bill.FirstOrDefault(rec => rec.Id == model.Id || rec.BillsNumber == model.BillsNumber);
                        if (element == null)
                        {
                            throw new Exception("Счет не найден");
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
        public void Delete(BillsBindingModels model)
        {
            using (var context = new BankYouBankruptDatabase())
            {
                var element = context.Bill.FirstOrDefault(rec => rec.Id == model.Id || rec.BillsNumber == model.BillsNumber);
                if (element != null)
                {
                    context.Bill.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Счет не найден");
                }
            }
        }
    }
}