using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankYouBankruptDatabaseImplement.Models
{
    public class Cards
    {
        public int Id { get; set; }
        //номер карты
        [Required]
        public string CardsNumder { get; set; }
        [Required]
        //код безопасности
        public int SecurityCode { get; set; }
        [Required]
        //дата окончания обслуживания
        public DateTime ServiceEndDate { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("CardId")]
        public virtual List<CardsApplication> CardsAplications { get; set; }

        [ForeignKey("CardId")]
        public virtual List<CardsOperation> CardsOperations { get; set; }


    }
}
