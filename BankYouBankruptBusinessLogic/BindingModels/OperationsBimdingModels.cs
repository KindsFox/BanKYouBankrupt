using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BindingModels
{
    public class OperationsBimdingModels
    {
        public int? Id { get; set; }
        public DateTime OperationDate { get; set; }
        public int OperationNumber { get; set; }
        public string OperationType { get; set; }
        public int? UserId { get; set; }
    }
}
