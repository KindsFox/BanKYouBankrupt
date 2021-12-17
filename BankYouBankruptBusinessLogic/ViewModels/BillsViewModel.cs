using System.ComponentModel;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class BillsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Номер счета")]       
        public string BillsNumber { get; set; }
        [DisplayName("Остаток на счете")]
        public float BillsBalance { get; set; }
        public Dictionary<int, string> MoneyTransferId { get; set; }
        public Dictionary<int, bool> BillCashWithdrawalId { get; set; }
    }
}
