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
    public class CardsStorage : ICardsStorage
    {
        public Cards CreateModel(CardsBindingModels model, Cards cards, BankYouBankruptDatabase context)
        {
            cards.CardsNumder = model.CardsNumder;
            cards.SecurityCode = model.SecurityCode;
            cards.ServiceEndDate = model.ServiceEndDate;
            cards.UserId = (int)model.UserId;
            if (cards.Id == 0)
            {
                context.Card.Add(cards);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                List<CardsApplication> cardsApplication = context.CardsApplications.Where(rec => rec.CardId == model.Id.Value).ToList();
                List<CardsOperation> cardsOperations = context.CardsOperations.Where(rec => rec.CardId == model.Id.Value).ToList();
                context.CardsApplications.RemoveRange(cardsApplication.Where(rec => !model.CardsAplications.ContainsKey(rec.AplicationId)).ToList());
                context.SaveChanges();
                context.CardsOperations.RemoveRange(cardsOperations.Where(rec => !model.CardsOperations.ContainsKey(rec.OperationId)).ToList());
                context.SaveChanges();

                foreach (CardsOperation cardOperation in cardsOperations)
                {
                    if (model.CardsOperations.ContainsKey(cardOperation.OperationId))
                    {
                        model.CardsOperations.Remove(cardOperation.OperationId);
                    }
                }
                context.SaveChanges();

                foreach (CardsApplication cardApplication in cardsApplication)
                {
                    if (model.CardsAplications.ContainsKey(cardApplication.AplicationId))
                    {
                        model.CardsAplications.Remove(cardApplication.AplicationId);
                    }
                }
                context.SaveChanges();
            }

            foreach (KeyValuePair<int, decimal> CSP in model.CardsAplications)
            {
                context.CardsApplications.Add(new CardsApplication
                {
                    CardId = cards.Id,
                    AplicationId = CSP.Key
                });
                context.SaveChanges();
            }

            foreach (KeyValuePair<int, string> CSP in model.CardsOperations)
            {
                context.CardsOperations.Add(new CardsOperation
                {
                    CardId = cards.Id,
                    OperationId = CSP.Key
                });
                context.SaveChanges();
            }
            return cards;
        }

        public CardsViewModel GetElement(CardsBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                Cards cards = context.Card
                    .Include(rec => rec.CardsAplications)
                    .ThenInclude(rec => rec.Application)
                    .Include(rec => rec.CardsOperations)
                    .ThenInclude(rec => rec.Operation)
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.CardsNumder == model.CardsNumder);
                return cards != null ?
                new CardsViewModel
                {
                    Id = cards.Id,
                    CardsNumber = cards.CardsNumder,
                    SecurityCode = cards.SecurityCode,
                    ServiceEndDateString = string.Format("{0}/{1}", cards.ServiceEndDate.Month, cards.ServiceEndDate.Year),
                    ServiceEndDate = cards.ServiceEndDate,
                    UserId = cards.UserId,
                    UserFIO = cards.User.FIO,
                    CardsAplications = cards.CardsAplications.ToDictionary(recCSR => recCSR.AplicationId, recCSR => recCSR.Application.AplicationSum),
                    CardsOperations = cards.CardsOperations.ToDictionary(recCSR => recCSR.OperationId, recCSR => recCSR.Operation.OperationType)
                } :
                null;
            }
        }

        public List<CardsViewModel> GetFilteredList(CardsBindingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.Card
                    .Include(rec => rec.CardsAplications)
                    .ThenInclude(rec => rec.Application)
                    .Include(rec => rec.CardsOperations)
                    .ThenInclude(rec => rec.Operation)
                    .Include(rec => rec.User)
                    .Where(rec => rec.CardsNumder.Equals(model.CardsNumder)
                    || (model.UserId.HasValue && rec.UserId == model.UserId)
                    || (model.ApplicationId.HasValue && rec.CardsAplications.Any(recw => recw.AplicationId == model.ApplicationId))
                    || (model.OperationId.HasValue && rec.CardsOperations.Any(recw => recw.OperationId == model.OperationId))
                    || (model.SelectedCards != null && (rec.CardsOperations.Any(recTMW => model.SelectedCards.Contains(recTMW.OperationId)))))
                    .ToList()
                    .Select(rec => new CardsViewModel
                    {
                        Id = rec.Id,
                        CardsNumber = rec.CardsNumder,
                        SecurityCode = rec.SecurityCode,
                        ServiceEndDateString = string.Format("{0}/{1}", rec.ServiceEndDate.Month, rec.ServiceEndDate.Year),
                        ServiceEndDate = rec.ServiceEndDate,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        CardsAplications = rec.CardsAplications.ToDictionary(recCSR => recCSR.AplicationId, recCSR => recCSR.Application.AplicationSum),
                        CardsOperations = rec.CardsOperations.ToDictionary(recCSR => recCSR.OperationId, recCSR => recCSR.Operation.OperationType)
                    }).ToList();
            }
        }

        public List<CardsViewModel> GetFullList()
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.Card
                     .Include(rec => rec.CardsAplications)
                     .ThenInclude(rec => rec.Application)
                     .Include(rec => rec.CardsOperations)
                     .ThenInclude(rec => rec.Operation)
                     .Include(rec => rec.User)
                     .ToList()
                     .Select(rec => new CardsViewModel
                     {
                         Id = rec.Id,
                         CardsNumber = rec.CardsNumder,
                         SecurityCode = rec.SecurityCode,
                         ServiceEndDateString = string.Format("{0}/{1}", rec.ServiceEndDate.Month, rec.ServiceEndDate.Year),
                         ServiceEndDate = rec.ServiceEndDate,
                         UserId = rec.UserId,
                         UserFIO = rec.User.FIO,
                         CardsAplications = rec.CardsAplications.ToDictionary(recCSR => recCSR.AplicationId, recCSR => recCSR.Application.AplicationSum),
                         CardsOperations = rec.CardsOperations.ToDictionary(recCSR => recCSR.OperationId, recCSR => recCSR.Operation.OperationType)
                     }).ToList();
            }
        }

        public void Insert(CardsBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Cards(), context);
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

        public void Update(CardsBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Card.FirstOrDefault(rec => rec.Id == model.Id || rec.CardsNumder == model.CardsNumder);
                        if (element == null)
                        {
                            throw new Exception("Карта не найдена");
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
        public void Delete(CardsBindingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                var element = context.Card.FirstOrDefault(rec => rec.Id == model.Id || rec.CardsNumder == model.CardsNumder);
                if (element != null)
                {
                    context.Card.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Карта не найдена");
                }
            }
        }
    }
}
