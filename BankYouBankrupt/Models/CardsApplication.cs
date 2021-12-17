namespace BankYouBankruptDatabaseImplement.Models
{
    public class CardsApplication
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int AplicationId { get; set; }       
        public virtual Cards Card { get; set; }
        public virtual Applications Application { get; set; }
    }
}
