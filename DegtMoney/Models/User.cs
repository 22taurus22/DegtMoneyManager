using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DegtMoney.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Email { get; set; } = "";
        public string CurrencyCode { get; set; } = "RUB";
        public bool IsDarkTheme { get; set; }
        public string LanguageCode { get; set; } = "ru";

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();
        public virtual ICollection<BudgetLimit> BudgetLimits { get; set; } = new List<BudgetLimit>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
