using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankYouBankruptDatabaseImplement.Models
{
    //операции
    public class Operations
    {
        public int Id { get; set; }
        [Required]
        public DateTime OperationDate { get; set; }
        public string OperationType { get; set; }
        [Required]
        public int OperationNumber { get; set; }        
        public int UserId { get; set; }       
        public virtual User User { get; set; }
        [ForeignKey("OperationId")]
        public virtual List<CardsOperation> CardsOperations { get; set; }
        public virtual List<MoneyTransfer> MoneyTransfers { get; set; }
    }    
}
