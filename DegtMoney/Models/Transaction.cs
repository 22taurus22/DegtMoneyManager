using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string NoteEncrypted { get; set; } = "";
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public int? AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}
