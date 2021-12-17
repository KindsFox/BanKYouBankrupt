using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankYouBankruptDatabaseImplement.Models
{
    //выдача наличных
    public class CashWithdrawal
    {
        [Key]
        [ForeignKey("Applications")]
        public int ApplicationId { get; set; }
        [Required]
        //наличие заявки
        public bool AvailabilityApplication { get; set; }

        [ForeignKey("CashWithdrawalId")]
        public virtual List<BillCashWithdrawal> CashWithdrawalBills { get; set; }
        public virtual Applications Application { get; set; }

    }
}
