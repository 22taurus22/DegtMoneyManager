using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Models
{
    public class SavingGoal
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public bool IsAchieved { get; set; }
    }
}
