namespace BankYouBankruptDatabaseImplement.Models
{
    public class MoneyTransferBill
    {
        public int Id { get; set; }
        public int MoneyTransferId { get; set; }
        public int BillsId { get; set; }
        public virtual MoneyTransfer MoneyTransfer { get; set; }
        public virtual Bills Bills { get; set; }
    }
}
