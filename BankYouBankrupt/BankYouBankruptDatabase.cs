using Microsoft.EntityFrameworkCore;
using BankYouBankruptDatabaseImplement.Models;

namespace BankYouBankruptDatabaseImplement
{
    public class BankYouBankruptDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankYouBankruptDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Applications> Application { set; get; }
        public virtual DbSet<Bills> Bill { set; get; }
        public virtual DbSet<BillCashWithdrawal> CashWithdrawalBills { set; get; }
        public virtual DbSet<Cards> Card { set; get; }
        public virtual DbSet<CardsApplication> CardsApplications { set; get; }
        public virtual DbSet<CardsOperation> CardsOperations { set; get; }
        public virtual DbSet<CashWithdrawal> CashWithdrawals { set; get; }
        public virtual DbSet<MoneyTransfer> MoneyTransfers { set; get; }
        public virtual DbSet<MoneyTransferApplication> MoneyTransferApplications { set; get; }
        public virtual DbSet<MoneyTransferBill> MoneyTransferBills { set; get; }
        public virtual DbSet<Operations> Operation { set; get; }
        public virtual DbSet<User> Users { set; get; }
    }
}
