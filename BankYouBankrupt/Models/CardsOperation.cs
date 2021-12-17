namespace BankYouBankruptDatabaseImplement.Models
{
    public class CardsOperation
    {
        public int Id { get; set; }
        public int OperationId { get; set; }
        public int CardId { get; set; }
        public virtual Operations Operation { get; set; }
        public virtual Cards Card { get; set; }
    }
}
