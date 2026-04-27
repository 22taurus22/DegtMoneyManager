using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Models
{
    public class BudgetLimit
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public decimal MonthlyLimit { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
