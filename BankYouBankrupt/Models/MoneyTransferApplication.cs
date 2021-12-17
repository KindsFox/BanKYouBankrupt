namespace BankYouBankruptDatabaseImplement.Models
{
    public class MoneyTransferApplication
    {
        public int Id { get; set; }
        public int MoneyTransferId { get; set; }
        public int AplicationId { get; set; }
        public virtual Applications Applications { get; set; }
        public virtual MoneyTransfer MoneyTransfer { get; set; }
    }
}
