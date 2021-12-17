using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankYouBankruptDatabaseImplement.Models
{
    //клиент
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FIO { get; set; }        
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Number { get; set; }
        public virtual List<Applications> Aplications { get; set; }
        public virtual List<Operations> Operations { get; set; }
        public virtual List<Cards> Cards { get; set; }
    }
}
