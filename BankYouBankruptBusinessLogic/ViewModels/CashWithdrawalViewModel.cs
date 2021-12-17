using System.ComponentModel;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class CashWithdrawalViewModel
    {
        public int Id { get; set; }
        [DisplayName("Наличие заявки")]
        public bool AvailabilityApplication { get; set; } 
        public int AplicationsId { get; set; }
        [DisplayName("Номер заявки")]
        public int ApplicationNumber { get; set; }
    }
}
