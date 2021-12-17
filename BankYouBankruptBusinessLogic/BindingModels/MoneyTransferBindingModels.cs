using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BindingModels
{
    public class MoneyTransferBindingModels
    {
        public int? Id { get; set; }
        //от кого ФИО
        public string Sender { get; set; }
        //кому ФИО
        public string Recipient { get; set; }
        //номер карты отправителя
        public string SendersCard { get; set; }
        //номер карты получателя
        public string RecipientsCard { get; set; }          
        public int OperationId { get; set; }
    }
}
