using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankYouBankruptDatabaseImplement.Models
{
    //заявки
    public class Applications
    {        
        public int Id { get; set; }
        [Required]
        public decimal AplicationSum { get; set; }
        [Required]
        public DateTime AplicationDate { get; set; }
        [Required]
        public int AplicationNumber { get; set; }
        public int UserId { get; set; }        
        public virtual User User { get; set; }
        [ForeignKey("AplicationId")]
        public virtual List<MoneyTransferApplication> AplicationMoneyTransfer { get; set; }
        [ForeignKey("AplicationId")]
        public virtual List<CardsApplication> CardsAplications { get; set; }
        public virtual CashWithdrawal CashWithdrawals { get; set; }

    }
}
