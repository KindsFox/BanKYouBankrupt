namespace BankYouBankruptDatabaseImplement.Models
{
    public class BillCashWithdrawal
    {
        public int Id { get; set; }
        public int CashWithdrawalId { get; set; }
        public int BillsId { get; set; }
        public virtual CashWithdrawal CashWithdrawals { get; set; }
        public virtual Bills Bill { get; set; }
    }
}
