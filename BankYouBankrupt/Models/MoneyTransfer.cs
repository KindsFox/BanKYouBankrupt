using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankYouBankruptDatabaseImplement.Models
{
    //перевод денег
    public class MoneyTransfer
    {
        public int Id { get; set; }
        [Required]
        //от кого ФИО
        public string Sender { get; set; }
        [Required]
        //кому ФИО        
        public string Recipient { get; set; }
        [Required]
        //номер карты отправителя
        public string SendersCard { get; set; }
        [Required]
        //номер карты получателя
        public string RecipientsCard { get; set; }
        public int OperationId { get; set; }
        public string OperationType { get; set; }
        public virtual Operations Operation { get; set; }
        [ForeignKey("MoneyTransferId")]
        public virtual List<MoneyTransferApplication> AplicationMoneyTransfer { get; set; }
        [ForeignKey("MoneyTransferId")]
        public virtual List<MoneyTransferBill> MoneyTransferBills { get; set; }
        
    }
}
