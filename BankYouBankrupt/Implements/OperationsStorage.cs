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
    public class OperationsStorage : IOperationsStorage
    {
        public Operations CreateModel(OperationsBimdingModels model, Operations operation)
        {
            operation.OperationDate = model.OperationDate;
            operation.OperationNumber = model.OperationNumber;
            operation.OperationType = model.OperationType;
            operation.UserId = (int)model.UserId;
            return operation;
        }
        public OperationsViewModel GetElement(OperationsBimdingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                Operations operation = context.Operation
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id);
                return operation != null ?
                new OperationsViewModel
                {
                    Id = operation.Id,
                    OperationDate = operation.OperationDate,
                    OperationNumber = operation.OperationNumber,
                    OperationType = operation.OperationType,
                    UserId = operation.UserId,
                    UserFIO = operation.User.FIO
                } :
                null;
            }
        }

        public List<OperationsViewModel> GetFilteredList(OperationsBimdingModels model)
        {
            if (model == null)
            {
                return null;
            }
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.Operation
                    .Include(rec => rec.User)
                    .Where(rec => rec.OperationNumber.Equals(model.OperationNumber) || (model.UserId.HasValue && rec.UserId == model.UserId))
                    .ToList()
                    .Select(rec => new OperationsViewModel
                    {
                        Id = rec.Id,
                        OperationDate = rec.OperationDate,
                        OperationNumber = rec.OperationNumber,
                        OperationType = rec.OperationType,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }
        
        public List<OperationsViewModel> GetFullList()
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                return context.Operation
                    .Include(rec => rec.User)
                    .Select(rec => new OperationsViewModel
                   {
                        Id = rec.Id,
                        OperationDate = rec.OperationDate,
                        OperationNumber = rec.OperationNumber,
                        OperationType = rec.OperationType,
                        UserId = rec.UserId,
                       UserFIO = rec.User.FIO,
                    }).ToList();
                   
            }
        }
        
        public void Insert(OperationsBimdingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                context.Operation.Add(CreateModel(model, new Operations()));
                context.SaveChanges();
            }
        }

        public void Update(OperationsBimdingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                Operations serviceRecording = context.Operation.FirstOrDefault(rec => rec.Id == model.Id);
                if (serviceRecording == null)
                {
                    throw new Exception("Операция не найдена");
                }
                CreateModel(model, serviceRecording);
                context.SaveChanges();
            }
        }
        public void Delete(OperationsBimdingModels model)
        {
            using (BankYouBankruptDatabase context = new BankYouBankruptDatabase())
            {
                Operations serviceRecording = context.Operation.FirstOrDefault(rec => rec.Id == model.Id);
                if (serviceRecording != null)
                {
                    context.Operation.Remove(serviceRecording);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Операция не найдена");
                }
            }
        }
    }
}
