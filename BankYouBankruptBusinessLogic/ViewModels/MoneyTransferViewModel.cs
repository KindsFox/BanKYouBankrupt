using System.ComponentModel;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.ViewModels
{
    public class MoneyTransferViewModel
    {
        public int Id { get; set; }
        [DisplayName("Перевод от кого ФИО")]
        public string Sender { get; set; }
        [DisplayName("Перевод кому ФИО")]
        public string Recipient { get; set; }
        [DisplayName("Номер карты отправителя")]
        public string SendersCard { get; set; }
        [DisplayName("Номер карты получателя")]
        public string RecipientsCard { get; set; }
        public int OperationId { get; set; }
        [DisplayName("Номер операции")]
        public int OperationNumber { get; set; }
        
    }
}
