using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BindingModels
{
    //счета
    public class BillsBindingModels
    {
        public int? Id { get; set; }
        //номер счета
        public string BillsNumber { get; set; }
        //остаток на счете
        public float BillsBalance { get; set; }
               
        public Dictionary<int, string> BillsMoneyTransfer { get; set; }
        public Dictionary<int, bool> BillCashWithdrawalId { get; set; }
    }
}
