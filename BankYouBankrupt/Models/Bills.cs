using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankYouBankruptDatabaseImplement.Models
{
    //счета
    public class Bills
    {
        public int Id { get; set; }
        //остаток на счете
        [Required]
        public float BillsBalance { get; set; }
        //номер счета
        [Required]
        public string BillsNumber { get; set; }       
        [ForeignKey ("BillsId")]
        public virtual List<BillCashWithdrawal> CashWithdrawalBills { get; set; }
        [ForeignKey ("BillsId")]
        public virtual List<MoneyTransferBill> MoneyTransferBills { get; set; }

    }
}
